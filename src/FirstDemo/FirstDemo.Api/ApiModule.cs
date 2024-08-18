using Autofac;
using FirstDemo.Api.RequestHandlers;

namespace FirstDemo.Api
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewCourseRequestHandler>().AsSelf();

            base.Load(builder);
        }
    }
}
