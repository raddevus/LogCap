systemctl stop logcap.service
dotnet publish ./LogCap.csproj -c Release -o /var/www/logcap
systemctl start logcap.service
