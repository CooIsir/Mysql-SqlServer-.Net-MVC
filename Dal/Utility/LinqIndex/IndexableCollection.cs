using System;
using System.Linq;
using Ex = System.Linq.Expressions.Expression;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace com.Utility
{
    public class IndexableCollection<T> : Collection<T>
    {

        //this defines a dictionary of dictionaries of lists of some type we are being a collection of :)
        //the index is always the hash of whatever we are indexing.

        protected Dictionary<string, Dictionary<int, List<T>>> _indexes 
            = new Dictionary<string, Dictionary<int, List<T>>>();
        
        public IndexableCollection() : base()
        {
            BuildIndexes();
        }

        public IndexableCollection(IList<T> list)
            : base(list)
        {
            BuildIndexes();
        }

        public void BuildIndexes()
        {
            PropertyInfo[] allProps = typeof(T).GetProperties();
            foreach (PropertyInfo prop in allProps)
            {
                object[] attributes = prop.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                    if (attribute is IndexableAttribute)
                        _indexes.Add(prop.Name, new Dictionary<int, List<T>>());
            }
        }

        //public void InjectObjectChangeNotification(PropertyInfo prop)
        //{
        //    MethodInfo meth = prop.GetSetMethod(false);
        //    MethodInfo myPropChangeMethod = typeof(IndexableCollection<T>).GetMethod("PropertyChanged");
        //    PropertyBuilder propBuild = new PropertyBuilder();
        //    MethodBuilder methBuild = new MethodBuilder();

        //    ILGenerator ilGen = methBuild.GetILGenerator();
  
        //    methBuild.CreateMethodBody(meth.GetMethodBody().GetILAsByteArray(), meth.GetMethodBody().GetILAsByteArray().Length);
        //    ilGen.Emit(OpCodes.Ldarg_0, 0); //pushes "this" onto the stack
        //    ilGen.Emit(OpCodes.Ldarg_1, prop.Name);
        //    ilGen.EmitCall(OpCodes.Call, myPropChangeMethod, null);


        //}

        public void PropertyChanged(T theObject, string prop)
        {
            
        }

        public bool PropertyHasIndex(string propName)
        {
            return _indexes.ContainsKey(propName);
        }
        
        public Dictionary<int, List<T>> GetIndexByProperty(string propName)
        {
            return _indexes[propName];
        }

        public new void Add(T newItem)
        {
            foreach(string key in _indexes.Keys)
            {
                PropertyInfo theProp = typeof(T).GetProperty(key);
                if (theProp != null)
                {
                    int hashCode = theProp.GetValue(newItem, null).GetHashCode();
                    Dictionary<int, List<T>> index = _indexes[key];
                    if (index.ContainsKey(hashCode))
                        index[hashCode].Add(newItem);
                    else
                    {
                        List<T> newList = new List<T>(1);
                        newList.Add(newItem);
                        index.Add(hashCode, newList);
                    }
                }
            }
            base.Add(newItem);
        }
    }

    public static class IndexableCollectionExtention
    {
        public static IEnumerable<TResult> GroupJoin<T, TInner, TKey, TResult>(
            this IndexableCollection<T> outer,
            IndexableCollection<TInner> inner,
            Expression<Func<T, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Func<T, IEnumerable<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (outer == null || inner == null || outerKeySelector == null || innerKeySelector == null || resultSelector == null)
                throw new ArgumentNullException();

            bool haveIndex = false;

            if (innerKeySelector.NodeType == ExpressionType.Lambda
                && innerKeySelector.Body.NodeType == ExpressionType.MemberAccess
                && outerKeySelector.NodeType == ExpressionType.Lambda
                && outerKeySelector.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression membExpInner = (MemberExpression)innerKeySelector.Body;
                MemberExpression membExpOuter = (MemberExpression)outerKeySelector.Body;
                Dictionary<int, List<TInner>> innerIndex = new Dictionary<int, List<TInner>>();
                Dictionary<int, List<T>> outerIndex = new Dictionary<int, List<T>>();


                if (inner.PropertyHasIndex(membExpInner.Member.Name)
                    && outer.PropertyHasIndex(membExpOuter.Member.Name))
                {
                    innerIndex = inner.GetIndexByProperty(membExpInner.Member.Name);
                    outerIndex = outer.GetIndexByProperty(membExpOuter.Member.Name);
                    haveIndex = true;
                }

                if (haveIndex)
                {
                    foreach (int outerKey in outerIndex.Keys)
                    {
                        List<T> outerGroup = outerIndex[outerKey];
                        List<TInner> innerGroup;
                        if (innerIndex.TryGetValue(outerKey, out innerGroup))
                        {
                            //do a join on the GROUPS based on key result
                            IEnumerable<TInner> innerEnum = innerGroup.AsEnumerable<TInner>();
                            IEnumerable<T> outerEnum = outerGroup.AsEnumerable<T>();
                            IEnumerable<TResult> result = outerEnum.GroupJoin<T, TInner, TKey, TResult>(innerEnum, outerKeySelector.Compile(), innerKeySelector.Compile(), resultSelector);
                            foreach (TResult resultItem in result)
                                yield return resultItem;
                        }
                    }
                }
            }
            if (!haveIndex)
            {
                //do normal group join
                IEnumerable<TInner> innerEnum = inner.AsEnumerable<TInner>();
                IEnumerable<T> outerEnum = outer.AsEnumerable<T>();
                IEnumerable<TResult> result = outerEnum.GroupJoin<T, TInner, TKey, TResult>(innerEnum, outerKeySelector.Compile(), innerKeySelector.Compile(), resultSelector, comparer);
                foreach (TResult resultItem in result)
                    yield return resultItem;
            }
        }

        public static IEnumerable<TResult> GroupJoin<T, TInner, TKey, TResult>(
            this IndexableCollection<T> outer,
            IndexableCollection<TInner> inner,
            Expression<Func<T, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Func<T, IEnumerable<TInner>, TResult> resultSelector)
        {
            return outer.GroupJoin<T, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static IEnumerable<TResult> Join<T, TInner, TKey, TResult>(
            this IndexableCollection<T> outer,
            IndexableCollection<TInner> inner,
            Expression<Func<T, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (outer == null || inner == null || outerKeySelector == null || innerKeySelector == null || resultSelector == null)
                throw new ArgumentNullException();

            bool haveIndex = false;
            if (innerKeySelector.NodeType == ExpressionType.Lambda
                && innerKeySelector.Body.NodeType == ExpressionType.MemberAccess
                && outerKeySelector.NodeType == ExpressionType.Lambda
                && outerKeySelector.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression membExpInner = (MemberExpression)innerKeySelector.Body;
                MemberExpression membExpOuter = (MemberExpression)outerKeySelector.Body;
                Dictionary<int, List<TInner>> innerIndex = new Dictionary<int, List<TInner>>();
                Dictionary<int, List<T>> outerIndex = new Dictionary<int, List<T>>();

                if (inner.PropertyHasIndex(membExpInner.Member.Name)
                    && outer.PropertyHasIndex(membExpOuter.Member.Name))
                {
                    innerIndex = inner.GetIndexByProperty(membExpInner.Member.Name);
                    outerIndex = outer.GetIndexByProperty(membExpOuter.Member.Name);
                    haveIndex = true;
                }

                if (haveIndex)
                    foreach (int outerKey in outerIndex.Keys)
                    {
                        List<T> outerGroup = outerIndex[outerKey];
                        List<TInner> innerGroup;
                        if (innerIndex.TryGetValue(outerKey, out innerGroup))
                        {
                            //do a join on the GROUPS based on key result
                            IEnumerable<TInner> innerEnum = innerGroup.AsEnumerable<TInner>();
                            IEnumerable<T> outerEnum = outerGroup.AsEnumerable<T>();
                            IEnumerable<TResult> result = outerEnum.Join<T, TInner, TKey, TResult>(innerEnum, outerKeySelector.Compile(), innerKeySelector.Compile(), resultSelector, comparer);
                            foreach (TResult resultItem in result)
                                yield return resultItem;
                        }
                    }
            }
            if (!haveIndex)
            {
                //this will happen if we don't have keys in the right places
                IEnumerable<TInner> innerEnum = inner.AsEnumerable<TInner>();
                IEnumerable<T> outerEnum = outer.AsEnumerable<T>();
                IEnumerable<TResult> result = outerEnum.Join<T, TInner, TKey, TResult>(innerEnum, outerKeySelector.Compile(), innerKeySelector.Compile(), resultSelector, comparer);
                foreach (TResult resultItem in result)
                    yield return resultItem;
            }
        }

        public static IEnumerable<TResult> Join<T, TInner, TKey, TResult>(
            this IndexableCollection<T> outer,
            IndexableCollection<TInner> inner,
            Expression<Func<T, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Func<T, TInner, TResult> resultSelector)
        {
            return outer.Join<T, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        private static bool HasIndexablePropertyOnLeft<T>(Expression leftSide, IndexableCollection<T> sourceCollection)
        {
            if (leftSide.NodeType == ExpressionType.MemberAccess)
                return sourceCollection.PropertyHasIndex(((MemberExpression)leftSide).Member.Name);
            else
                return false;
        }

        private static int? GetHashRight(Expression leftSide, Expression rightSide)
        {
            //rightside is where we get our hash...
            switch (rightSide.NodeType)
            {
                //shortcut constants, dont eval, will be faster
                case ExpressionType.Constant:
                    ConstantExpression constExp
                        = (ConstantExpression)rightSide;
                    return (constExp.Value.GetHashCode());
                //case ExpressionType.MemberAccess:
                //    return null; //member expressions cant eval
                
                //if not constant (which is provably terminal in a tree), convert back to Lambda and eval to get the hash.
                default:
                    //Lambdas can be created from expressions... yay
                    LambdaExpression evalRight = Ex.Lambda(rightSide, null);
                    //Compile that mutherf-ker, invoke it, and get the resulting hash
                    return (evalRight.Compile().DynamicInvoke(null).GetHashCode());
            }
        }
                
        //extend the where when we are working with indexable collections! 
        public static IEnumerable<T> Where<T>
        (
            this IndexableCollection<T> sourceCollection,
            Expression<Func<T, bool>> expr
            )
        {
            //our indexes work from the hash values of that which is indexed, regardless of type
            int? hashRight = null;
            bool noIndex = true;
            //indexes only work on equality expressions here
            if (expr.Body.NodeType == ExpressionType.Equal)
            {
                //Equality is a binary expression
                BinaryExpression binExp = (BinaryExpression)expr.Body;
                //Get some aliases for either side
                Expression leftSide = binExp.Left;
                Expression rightSide = binExp.Right;

                hashRight = GetHashRight(leftSide, rightSide);

                //if we were able to create a hash from the right side (likely)
                if (hashRight.HasValue && HasIndexablePropertyOnLeft<T>(leftSide,sourceCollection))
                {
                    //cast to MemberExpression - it allows us to get the property
                    MemberExpression propExp = (MemberExpression)leftSide;
                    string property = propExp.Member.Name;
                    Dictionary<int, List<T>> myIndex =
                            sourceCollection.GetIndexByProperty(property);
                    if (myIndex.ContainsKey(hashRight.Value))
                    {
                        IEnumerable<T> sourceEnum = myIndex[hashRight.Value].AsEnumerable<T>();
                        IEnumerable<T> result = sourceEnum.Where<T>(expr.Compile());
                        foreach (T item in result)
                            yield return item;
                    }
                    noIndex = false; //we found an index, whether it had values or not is another matter
                }

            }
            if (noIndex) //no index?  just do it the normal slow way then...
            {
                IEnumerable<T> sourceEnum = sourceCollection.AsEnumerable<T>();
                IEnumerable<T> result = sourceEnum.Where<T>(expr.Compile());
                foreach (T resultItem in result)
                    yield return resultItem;
            }
        }
    }
}
