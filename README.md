# Token Authentication in ASP.NET Web API

This project is about implementing the token authentication security feature in ASP.NET Web API using the identity framework provided by the .Net framework.Data from the authorized api pages of the application can be fetched by registered user with a valid token only. Client pages with jquery ajax are included in the project to test the project without any help of third party applications.

## Technologies

Project is created with:
* ASP.NET Web API
* Entity Framework
* MS Sql Server
* Jquery Ajax

## Implementation

*Registering Users
Out of the box .NET provided a registration method with identity frmework.

```  public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {  return BadRequest(ModelState);  }
            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            { return GetErrorResult(result); }
            return Ok();
        }
```
Passing username and password to this method will save the details in identity database. This database will be created within the appliacation . This can be moved to sql server with some modification in the web.config file.Ajax call for the registration method is written below

```
$.ajax({
                    url: '/api/account/register',
                    method: 'POST',
                    data: {
                        email: $('#txtEmail').val(),
                        password: $('#txtPassword').val(),
                        confirmPassword: $('#txtConfirmPassword').val()
                    },
                    success: function () {
                        $('#successModal').modal('show');
                    },
                    error: function (jqXHR) {
                        $('#divErrorText').text(jqXHR.responseText);
                        $('#divError').show('fade');
                    }
                });
```
*Generating Token

The following auth configuration helping the user to genrate and modify the token.
```
public void ConfigureAuth(IAppBuilder app)
        {
        app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
      app.UseOAuthBearerTokens(OAuthOptions);
      }
```

Ajax call for the Token is shown below.The token in the response will be saved in the session storage.

```
 $.ajax({
                    url: '/token',
                    method: 'POST',
                    contentType: 'application/json',
                    data: {
                        username: $('#txtUsername').val(),
                        password: $('#txtPassword').val(),
                        grant_type: 'password'
                    },       
                    success: function (response) {
                        sessionStorage.setItem("accessToken", response.access_token);
                        window.location.href = "Data.html";
                    },
                    error: function (jqXHR) {
                        $('#divErrorText').text(jqXHR.responseText);
                        $('#divError').show('fade');
                    }
                });
```
Ajax call with the token which is saved in the session storage to fetch the data from the authorized page.
```                   url: '/api/Employee',
                    method: 'GET',
                    headers: {
                        'Authorization': 'Bearer '
                            + sessionStorage.getItem("accessToken")
```

## Contributing

Pull requests are welcome.

## License
