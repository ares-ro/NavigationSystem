using Microsoft.Extensions.Logging;

namespace NavigationSystem
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    //폰트파일명(혹은 적용 후 사용하기 위한 이름)이 한글이면 안됨...
                    fonts.AddFont("gct_Light.ttf", "gct_Light");
                    fonts.AddFont("gct_Medium.ttf", "gct_Medium");
                    fonts.AddFont("gct_Bold.ttf", "gct_Bold");

                    fonts.AddFont("NanumSquareL.ttf", "NanumSquareL");
                    fonts.AddFont("NanumSquareR.ttf", "NanumSquareR");
                    fonts.AddFont("NanumSquareB.ttf", "NanumSquareB");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
