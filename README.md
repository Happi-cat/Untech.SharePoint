# Untech.SharePoint

Untech.SharePoint - library that will improve your work with Lists in SharePoint (can be used with SSOM and CSOM)

## Project Structure

* Untech.SharePoint.Common - library that contains some global and common code (like MetaModels, Annotation mappings, common FieldConverters, SP to Linq providers and etc.)

* Untech.SharePoint.Client - contains CSOM specific code (i.e. SPClientContext and etc.)

* Untech.SharePoint.Server - contains SSOM specific code (i.e. SPServerContext and etc.)


## Example

* SSOM:

```cs
	var cfg = ServerConfig.Begin()
		.RegisterMappings(n => n.Annotated<InvestmenFrameworkContext>())
		.BuildConfig();

	// ...

	var web = new SPSite("http://localhost/sites/some").OpenWeb();
	var ctx = new InvestmenFrameworkContext(web, cfg);

	// ...

	var result = ctx.InvestmentProjects
		.Where(n => n.ProjectUniqueId.StartsWith("TTT") && n.Status == "Approved")
		.Where(n => n.Title.Contains("LALA"))
		.ToList();
```

* CSOM:


```cs
	var cfg = ClientConfig.Begin()
		.RegisterMappings(n => n.Annotated<InvestmenFrameworkContext>())
		.BuildConfig();

	// ...

	var clientCtx = new ClientContext("http://spserver/sites/some");
	var ctx = new InvestmenFrameworkContext(clientCtx, cfg);

	// ...

	var result = ctx.InvestmentProjects
		.Where(n => n.ProjectUniqueId.StartsWith("TTT") && n.Status == "Approved")
		.Where(n => n.Title.Contains("LALA"))
		.ToList();
```
