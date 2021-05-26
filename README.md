### DynamicLocalizer

> .net core dynamic localizer for any resource

#### Example

> use in Startup.cs with any resource by config

- load resource from db with EFCore as below

```c#
services.AddDynamicLocalizer(new DynamicLocalizerOption()
{
    LoadResource = () =>
    {
        //create EFCore db context
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Db"));
        var context = new ApplicationDbContext(optionsBuilder.Options);
      
        //demo data
        if (!context.Resource.Any())
        {
            context.Resource.AddRange(new List<Resource>()
            {
                new Resource()
                {
                    Code = "test",
                    Culture = "zh_CN",
                    Text = "zh_CN:测试"
                },
                new Resource()
                {
                    Code = "test",
                    Culture = "en_US",
                    Text = "en_US:Test"
                },
                new Resource()
                {
                    Code = "test",
                    Culture = "zh-CN",
                    Text = "zh-CN:测试"
                }
            });
            context.SaveChanges();
        }
      
        var resource = context.Resource.ToDictionary(e => $"{e.Code}.{e.Culture}", e => e.Text);

        return resource;
    },
    FormatCulture = (e => e.ToString().Replace("-", "_")),
    DefaultCulture = "zh_CN"
});

app.UseDynamicLocalizer();
```



#### Description

- DynamicLocalizerOption

  | option         | type                             | description                                                  | default                                |
  | -------------- | -------------------------------- | ------------------------------------------------------------ | -------------------------------------- |
  | LoadResource   | Func<Dictionary<string, string>> | function to load resource                                    | () => new Dictionary<string, string>() |
  | FormatCulture  | Func<CultureInfo, string>        | function to format culture from CultureInfo.CurrentCulture to Resource Culture | e => e.ToString()                      |
  | DefaultCulture | string                           | default culture                                              | "zh_CN"                                |

  
