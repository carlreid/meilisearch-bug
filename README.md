# Timeout Meilisearch Bug

Thanks to [@Jure-BB](https://github.com/Jure-BB) for the steps [listed here](https://github.com/meilisearch/MeiliSearch/issues/1555#issuecomment-923837429).

## Requires
- WSL2
  - docker
  - docker-compose
- .NET5

## How to reproduce
Ensure you have docker (tested using docker in WSL2) and docker-compose.

### Windows -> WSL -> Docker
1) Spin up meilisearch using `docker-compose up` inside WSL2
2) Run the "RunMe" project from Windows: `dotnet run` or from Visual Studio/Code

Expected: A timeout should happen after some time. This could be related to some Windows <-> WSL networking problem?
          A manual browser visit to `http://localhost:7700/indexes/books/stats` will result in a stats hit. Not what we expect.


### WSL -> Docker
1) Spin up meilisearch using `docker-compose up` inside WSL2
2) Publish the "RunMe" project for Linux (wherver you have `dotnet` installed): `dotnet publish -c Release -r linux-x64 --self-contained true`
3) Run the binary: `./bin/Release/net5.0/linux-x64/RunMe` (inside WSL)

Expected: We'd expect the timeout to also happen inside of Linux, but it doesn't happen.