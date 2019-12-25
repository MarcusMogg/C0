实现基础c0部分

## 编译项目

linux下

```
dotnet publish -r linux-x64 -c Release --self-contained -o publish
``

## 运行项目

```
./publish/cc0 [-s|-c] inputfile [-o outputfile]
```