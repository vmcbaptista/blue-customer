FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /src
COPY ["source/BlueCustomer.Api/BlueCustomer.Api.csproj", "source/BlueCustomer.Api/"]
COPY ["source/BlueCustomer.Core/BlueCustomer.Core.csproj", "source/BlueCustomer.Core/"]
COPY ["source/BlueCustomer.Infrastructure/BlueCustomer.Infrastructure.csproj", "source/BlueCustomer.Infrastructure/"]
COPY ["tests/BlueCustomer.Api.IntegrationTests/BlueCustomer.Api.IntegrationTests.csproj", "tests/BlueCustomer.Api.IntegrationTests/"]
RUN dotnet restore "tests/BlueCustomer.Api.IntegrationTests/BlueCustomer.Api.IntegrationTests.csproj"
COPY . .
WORKDIR "/src/tests/BlueCustomer.Api.IntegrationTests"
RUN curl https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh > /src/tests/BlueCustomer.Api.IntegrationTests/wait_for_it.sh