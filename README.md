### DynamicLocalizer

> .net core dynamic localizer

##### example

```c#
services.AddDynamicLocalizer(new DynamicLocalizerOption()
{
    LoadResource = () =>
    {
        var resource = new Dictionary<string, string>
        {
            { "test.zh_CN", "zh_CN:测试" },
            { "test.en_US", "en_US:Test" },
            { "test.zh-CN", "zh-CN:测试" },
        };
        return resource;
    },
    FormatCulture = (e => e.ToString().Replace("-", "_")),
    DefaultCulture = "zh_CN"
});

app.UseDynamicLocalizer();
```

