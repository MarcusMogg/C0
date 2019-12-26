实现基础c0部分+注释+字符串字面量+char类型转换

## 编译项目

linux下

```
chmod +x build.sh
./build.sh
```

## 运行项目

```
chmod +x cc0
./cc0 [-s|-c] inputfile [-o outptfile]
```

`-s|-c` 二选一，不选不输出

`inputfile`必须指定

`-o outptfile` 不指定时默认为out


## 帮助文档

```
  -s              将输入的 c0 源代码翻译为文本汇编文件

  -c              将输入的 c0 源代码翻译为二进制目标文件

  -o              (Default: out) 输出到指定的文件 file

  --help          Display this help screen.

  --version       Display version information.

  value pos. 0    (Default: test.c0) 源文件
```