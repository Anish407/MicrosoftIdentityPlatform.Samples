<h1>Secure the Web App </h1>

1. When the Register an application page appears, enter your application's registration information:
	Enter a Name for your application, 
	for example AspNetCore-WebApp. Users of your app might see this name, and you can change it later.

	a. Under Manage, select Authentication and then add the following information:
	b.  In the Web section, add https://localhost:7151/signin-oidc as a Redirect URI.
	c. In Front-channel logout URL, enter https://localhost:7151/signout-oidc.
	d. Under Implicit grant and hybrid flows, select ID tokens and Select Save.

2. Web applications that sign in users by using the Microsoft identity platform are configured through configuration files. These are the values you're required to specify in the configuration:
   
			The cloud instance (Instance) if you want your app to run in national clouds, for example
			The audience in the tenant ID (TenantId)
			The client ID (ClientId) for your application, as copied from the Azure portal
Link to <a href="./MIP.WebApp/appsettings.json"> appsettings.json</a> 

3. Initialization code
   a. Add the Microsoft.Identity.Web and Microsoft.Identity.Web.UI NuGet packages to your project. 
       Remove the Microsoft.AspNetCore.Authentication.AzureAD.UI NuGet package if it is present.
   b. Update the code in ConfigureServices so that it uses the
       AddMicrosoftIdentityWebAppAuthentication and AddMicrosoftIdentityUI methods.
       link
4. When your web app redirects the user to the logout endpoint, this endpoint clears the user's session from the browser. If your app didn't go to the logout endpoint, the user will reauthenticate to your app without entering their credentials again. The reason is that they'll have a valid single sign-in session with the Microsoft identity platform.
    To learn more, see the Send a sign-out request section in the Microsoft identity platform and the OpenID Connect protocol documentation.


Reference:
1. https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-web-app-sign-user-app-registration?tabs=aspnetcore
	
