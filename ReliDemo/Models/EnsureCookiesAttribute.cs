/// <summary>
/// Ensures that cookies are enabled.
/// </summary>
/// <exception cref="CookiesNotEnabledException" />
using System;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web;
using System.Runtime.Serialization;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class EnsureCookiesAttribute : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IAuthorizationFilter
{
    private readonly string _cookieName;
    private readonly bool _specificCookie;

    /// <summary>
    /// The name of the cookie to use to ensure cookies are enabled.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible",
        Justification = "Field is public so that the default value may be modified.")]
    public static string DefaultCookieName = "SupportsCookies";

    public const string CookieCheck = "cookieCheck";

    /// <summary>
    /// Checks to make sure cookies are generally enabled.
    /// </summary>
    public EnsureCookiesAttribute() : this(null) { }

    /// <summary>
    /// Checks to make sure a cookie with the given name exists
    /// </summary>
    /// <param name="cookieName">The name of the cookie</param>
    public EnsureCookiesAttribute(string cookieName)
    {
        if (String.IsNullOrEmpty(cookieName))
        {
            cookieName = DefaultCookieName;
        }
        else
        {
            _specificCookie = true;

        }

        QueryString = CookieCheck;

        _cookieName = cookieName;
    }

    /// <summary>
    /// The name of the cookie to check for.
    /// </summary>
    public string CookieName
    {
        get { return _cookieName; }
    }

    /// <summary>
    /// The querystring parameter to use to see if a test cookie has been set.
    /// </summary>
    public string QueryString { get; set; }

    protected static CookiesNotEnabledException CreateBrowserException()
    {
        return new CookiesNotEnabledException("Your browser does not support cookies.");
    }

    protected static CookiesNotEnabledException CreateNotEnabledException()
    {
        return new CookiesNotEnabledException("You do not have cookies enabled.");
    }

    #region Implementation of IAuthorizationFilter

    /// <summary>
    /// Called when authorization is required.
    /// </summary>
    /// <param name="filterContext">The filter context.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"
        , Justification = "Should swallow exceptions if a cookie can't be set.  This is the purpose of the filter.")]
    public void OnAuthorization(AuthorizationContext filterContext)
    {
        if (filterContext == null)
            throw new ArgumentNullException("filterContext");

        var request = filterContext.HttpContext.Request;
        var response = filterContext.HttpContext.Response;

        if (!request.Browser.Cookies)
            throw CreateBrowserException();

        string currentUrl = request.RawUrl;

        var noCookie = (request.Cookies[CookieName] == null);
        if (!_specificCookie && noCookie && request.QueryString[QueryString] == null)
        {
            try
            {
                // make it expire a long time from now, that way there's no need for redirects in the future if it already exists
                var c = new HttpCookie(CookieName, "true") { Expires = DateTime.Today.AddYears(50) };
                response.Cookies.Add(c);

                currentUrl = currentUrl + (currentUrl.Contains("?") ? "&" : "?") + QueryString + "=true";

                filterContext.Result = new RedirectResult(currentUrl);
                return;
            }
            catch
            {
            }
        }

        if (noCookie)
            throw CreateNotEnabledException();
    }

    #endregion
}

/// <summary>
/// Thrown when cookies are not supported.
/// </summary>
[Serializable]
public class CookiesNotEnabledException : HttpException
{
    public CookiesNotEnabledException()
    {
    }

    protected CookiesNotEnabledException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public CookiesNotEnabledException(string message)
        : base(message)
    {
    }

    public CookiesNotEnabledException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}