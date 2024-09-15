# Dotnet-ef bağımlıklıları yüklemek için 1 kez çalıştırılır.
dotnet tool install --global dotnet-ef --version 8.0.8

# Initial Mig işlemi için aşağıdaki komut çalıştırılır.
dotnet-ef migrations add Initial --project ./Api/JobStreamline.Api/

# Daha sonraki Mig işlemleri için aşağıdaki örnekler kullanılaiblir
dotnet-ef migrations add Migration_V001 --project ./Api/JobStreamline.Api
dotnet-ef database update --project ./Api/JobStreamline.Api