# my-first-tg-bot

#simple guide
https://telegrambots.github.io/book/index.html

#ASP.NET Core
https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Telegram.Bot.Examples.WebHook

#more examples
https://github.com/TelegramBots/Telegram.Bot.Examples
https://github.com/TelegramBotsAPI/Telegram.Bots


sudo docker run --rm --name pg-docker \
-e POSTGRES_PASSWORD=docker -d \
-p 5432:5432 \
-v /home/kinwend17/docker/volumes/postgres_data:/var/lib/postgresql/data \
-v /home/kinwend17/docker/volumes/postgres_backups:/backups \
-v /home/kinwend17/docker/volumes/postgres_init:/docker-entrypoint-initdb.d \
postgres:11.5


sudo docker exec -it pg-docker psql -U postgres -d amr-note-identity -f /backups/identity.sql
sudo docker exec -it pg-docker psql -U postgres -d amr-note-data -f /backups/data.sql
migrations

cd Infrastructure.Identity
dotnet-ef migrations add Initial --startup-project ../WebApi/WebApi.csproj -c "IdentityContext"



cd Infrastructure.Persistence
dotnet-ef migrations add Initial --startup-project ../WebApi/WebApi.csproj -c "ApplicationDbContext"


Remove last migrations

dotnet ef migrations remove --force --startup-project ../WebApi/WebApi.csproj -c "ApplicationDbContext"