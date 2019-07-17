# DotNetFramework.CAP
DotNetFramework.CAP 是一个基于 .NET Framework的 C# 库，它是一种处理分布式事务的解决方案,基于DotNetCore.CAP修改。

1. 此代码是基于DotCore.CAP 2.5.1 版本修改.

2. DotNetFramework.CAP 新增Core文件夹主要实现 DotNetCore下的Ioc容器. 日志Logger.

    a.  使用 AutoFac 实现 ServiceProvider,ServiceCollection,ServiceScope,ActivatorUtilities.
  
    b.  使用 Serilog 实现 Core下的Logger.
  
  
3. 内部代码修改如下：

    a. 删除DashBoard.暂时没有实现。
  
    b. 启动配置修改。
  
Exp.
  ./App_Srart
    public class CapConfig
    {
        public static IServiceCollection Services { get; set; }

        public static void RegisterCap()
        {
            Services = new ServiceCollection();
            Services.AddCap(stetup =>
            {
                // 注册节点到 Consul
                stetup.UseSqlServer("Data Source=localhost;database=donet61;Uid=sa;pwd=sa;");
                stetup.UseRabbitMQ(option =>
                {
                    option.VirtualHost = "HengQueue";
                    option.HostName = "localhost";
                    option.Port = 5672;
                    option.UserName = "zhangheng";
                    option.Password = "123456";
                });
            });
            Services.BeginRegister();
            Services.ServiceProvider.GetService<IBootstrapper>().BootstrapAsync(new CancellationToken());
        }
    }
    protected void Application_Start()
    {
        CapConfig.RegisterCap();
    }
    
    c.  获取controller下订阅方法修改。
     （这里由于.net core asp.net 和 framework asp.net的web机制变化）
            //heng
            //var types = Assembly.GetEntryAssembly().ExportedTypes;
            var types = BuildManager.GetGlobalAsaxType().BaseType.Assembly.ExportedTypes;
    d. Dapper执行Sql （将异步执行改为同步，因为发现在frameworkwork下会卡死）
     connection.Execute(sql);
     
    e. Sqlserver执行操作的发布消息时机的改动。
    
    Diagnostic.DiagnosticSource
    由于原作者（DoNetCoreCAP基本Core下Sqlserver的Diagnostic，完成的观测时机进行发布），
    framework下Sqlserver Client没有实  现Diagnostic的可观测行为。    
    
    修改为：   public static void Commit(this IDbTransaction trans, ICapPublisher bus)
              {
                  bus.Transaction.Commit();
              }
              public static void Commit(this DbContextTransaction trans, ICapPublisher bus)
              {
                  bus.Transaction.Commit();
              }
   提交事务使用如下代码：           
   transaction.Commit(_capBus);   详情参阅例子代码CapWeb251 
   
    

   
   
              
    
            
            
            
