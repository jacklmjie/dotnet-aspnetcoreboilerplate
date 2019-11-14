https://www.cnblogs.com/lwqlun/p/11155666.html
dotnet new -i <PATH> 安装模板目录
dotnet new ldp -n DemoPlugin2 创建新项目


nuget pack DemoPlugin1/LamondDynamicPlugin.nuspec  nuget打包
dotnet new -i LamondDynamicPlugin.nupkg 安装

Nuget包发布到nuget.org
dotnet new -i LamondDynamicPlugin

dotnet new -u 找到卸载的模板