using System;
using System.Security.Principal;
using System.Web.Security;
using System.Web;
using System.IO;
using System.Text;
using System.Configuration;

namespace com.Utility
{
	/// <summary>
	/// 帮助系统实现表单认证。
	/// </summary>
	/// <remarks>
	/// 可以在同一一级域名下不同服务器之间实现一次认证、多处访问。
	/// 需要在 web.config 文件中、在 <system.web /> 节下添加 <machineKey /> 节。
	/// 然后只要保证各个服务器的应用程序的 validationKey、decryptionKey、validation属性设置统一即可。
	/// </remarks>
	public class FormsAuth
	{
		private static int _expiration = 120;

		/// <summary>
		/// 获取服务器主机地址（包括访问协议方案、主机地址和端口号）。
		/// </summary>
		/// <value>服务器主机地址。</value>
		public static string HostUrl
		{
			get
			{
				Uri uri = HttpContext.Current.Request.Url;
				return String.Concat(uri.Scheme, "://", uri.Authority);
			}
		}

		/// <summary>
		/// 获取服务器应用程序虚拟路径。
		/// </summary>
		/// <value>服务器应用程序虚拟路径。</value>
		public static string ApplicationPath
		{
			get
			{
				string root = HttpContext.Current.Request.ApplicationPath;
				if (root!="/") root +="/";
				return root;
			}
		}

		/// <summary>
		/// 获取验证标志名。
		/// </summary>
		/// <value>验证标志名。</value>
		public static string FormName
		{
			get { return FormsAuthentication.FormsCookieName; }
		}

		/// <summary>
		/// 获取或设置用户身份证书过期时间。
		/// </summary>
		/// <value>用户身份证书过期时间。</value>
		public static int Expiration
		{
			get { return _expiration; }
			set { _expiration = value; }
		}

		/// <summary>
		/// 获取当前的认证用户。
		/// </summary>
		/// <returns>用户身份标识。</returns>
		public static string CurrentUserName
		{
			get{return HttpContext.Current.User.Identity.Name;}
		}

		/// <summary>
		/// 获取当前的帐套。
		/// </summary>
		/// <returns>当前的帐套</returns>
		public static string AccountID
		{
			get
			{
				string userID = HttpContext.Current.User.Identity.Name;
				if(userID.IndexOf("@")!=-1)
					return userID.Split('@')[1];
				else
                    return "39.net";
			}
		}
		/// <summary>
		/// 设置用户身份证书。
		/// </summary>
		/// <param name="name">用户身份标识。</param>
		/// <param name="roles">用户角色。</param>
		/// <param name="isPersistent">证书是否长期有效。</param>
		public static void SetAuthTicket(string name, string[] roles, bool isPersistent)
		{
			DateTime expires = (isPersistent) ? DateTime.MaxValue : DateTime.Now.AddMinutes(_expiration);

			string rolesString = String.Join(",", roles);
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, expires, isPersistent, rolesString);
			
			string sEncode = FormsAuthentication.Encrypt(ticket);
			HttpCookie cookie = new HttpCookie(FormName, sEncode);
			cookie.Domain = SLDomain;
			//cookie.Expires = expires;
			HttpContext.Current.Response.Cookies.Set(cookie);
		}

		/// <summary>
		/// 清除用户身份证书。
		/// </summary>
		public static void SignOut()
		{
			if(SLDomain!="")
			{
				HttpCookie cookie = new HttpCookie(FormName);
				cookie.Domain = SLDomain;
				cookie.Expires=DateTime.Now;
				HttpContext.Current.Response.Cookies.Set(cookie);
			}
			else	FormsAuthentication.SignOut();
		}

		// 获取当前访问的二级域名。
		public static string SLDomain
		{
			get
			{
				string domain = "";
				Uri url = HttpContext.Current.Request.Url;
				if (url.HostNameType == UriHostNameType.Dns)
				{
					string[] nodes = url.Host.ToLower().Split('.');
					if (nodes.Length > 2)
					{
						string lastNode = nodes[nodes.Length - 1];
						domain = String.Format("{0}.{1}", nodes[nodes.Length - 2], lastNode);
						if (lastNode != "com" && lastNode != "net" && lastNode != "org" && lastNode != "co")
						{
							domain = String.Format("{0}.{1}", nodes[nodes.Length - 3], domain);
						}
					}
				}
				return domain;
			}
		}

		/// <summary>
		/// 根据用户请求页面自动转向。
		/// </summary>
		public static void Redirect()
		{
			HttpContext.Current.Response.Redirect(GetRedirectUrl(), true);
		}

		/// <summary>
		/// 获取导致重定向到登录页的原始请求的重定向URL。
		/// </summary>
		/// <returns>导致重定向到登录页的原始请求的重定向URL。</returns>
		public static string GetRedirectUrl()
		{
			string host = HttpContext.Current.Request["ReturnHost"];
			string url = FormsAuthentication.GetRedirectUrl("*", false);
			if (host != null)
			{
				url = host + url;
			}
			return url;
		}

		/// <summary>
		/// 替换用户，每次请求时扮演关联的角色。
		/// </summary>
		/// <remarks>
		/// 应在 Global.asax 的 Application_OnAuthenticationRequest 函数中调用该方法，从而
		/// 在需要认证的Form设置用户与角色的关联。
		/// </remarks>
		public static void ImpersonateUser()
		{
			HttpContext currentContext = HttpContext.Current;
			if (currentContext.User != null)
			{
				if (currentContext.User.Identity is FormsIdentity && currentContext.User.Identity.IsAuthenticated)
				{
					FormsIdentity id = currentContext.User.Identity as FormsIdentity;
					FormsAuthenticationTicket ticket = id.Ticket;
					string[] roles = ticket.UserData.Split(',');
					currentContext.User = new GenericPrincipal(id, roles);
				}
			}
		}

		/// <summary>
		/// 设置与用户关联的角色。
		/// </summary>
		/// <param name="roles">用户的角色。</param>
		/// <remarks>
		/// 应在 Global.asax 的 Session_OnStart 函数中调用该方法，从而
		/// 改变用户的角色信息。
		/// </remarks>
		public static void SetUserRoles(string[] roles)
		{
			HttpContext currentContext = HttpContext.Current;
			if (currentContext.User != null)
			{
				if (currentContext.User.Identity is FormsIdentity && currentContext.User.Identity.IsAuthenticated)
				{
					FormsIdentity id = currentContext.User.Identity as FormsIdentity;
					
					SetAuthTicket(id.Name, roles, false);
					
					FormsAuthenticationTicket ticket = id.Ticket;
					currentContext.User = new GenericPrincipal(id, roles);
				}
			}
		}

		/// <summary>
		/// 返回一个值，指示当前用户是否在指定的角色中。
		/// </summary>
		/// <param name="role">用户角色。</param>
		/// <returns>用户属于指定的角色则返回 true；否则返回 false。</returns>
		public static bool IsInRole(string role)
		{
			return HttpContext.Current.User.IsInRole(role);
		}

		public static void SetCookies(
			string domain , 
			string realName,
			string roles
			)
		{
			HttpCookie hc = new HttpCookie("LoginAccountID",AccountID);
			hc.Domain = domain;
			HttpContext.Current.Response.AppendCookie(hc);
			
			hc=new HttpCookie(AccountID+"_RealName",realName);
			hc.Domain=domain;
			HttpContext.Current.Response.AppendCookie(hc);

			hc=new HttpCookie(AccountID+"_Roles",roles);
			hc.Domain=domain;
			HttpContext.Current.Response.AppendCookie(hc);
		}

		public static string RealName
		{
			get{ HttpCookie hc=HttpContext.Current.Request.Cookies[AccountID+"_RealName"]; return hc==null?string.Empty:hc.Value;}
			set{ HttpContext.Current.Request.Cookies[AccountID+"_RealName"].Value = value;}
		}
		public static string Roles
		{
			get{ HttpCookie hc=HttpContext.Current.Request.Cookies[AccountID+"_Roles"]; return hc==null?string.Empty:hc.Value;}
			set{ HttpContext.Current.Request.Cookies[AccountID+"_Roles"].Value = value;}
		}
	}
}
