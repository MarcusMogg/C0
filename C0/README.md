实现基础c0部分+注释+字符串字面量+char类型转换

## 编译项目

linux下

```
dotnet publish -r linux-x64 -c Release --self-contained -o cc0 /p:PublishSingleFile=true
```

## 运行项目

```
./cc0 [-s|-c] inputfile [-o outputfile]
```