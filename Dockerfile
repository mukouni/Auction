# Stage 1 - Restoring & Compiling
FROM mcr.microsoft.com/dotnet/core/sdk as builder
# RUN apt-get update \ 
#     && apt-get install -y net-tools
WORKDIR /app
#RUN apk add --update nodejs nodejs-npm
COPY *.csproj .
RUN dotnet restore
#COPY package.json .
#RUN npm install
COPY . .
RUN dotnet publish -c Release -o out

# Stage 2 - Creating Image for compiled app
FROM mcr.microsoft.com/dotnet/core/runtime as runtime
# RUN apt-get update \ 
#     && apt-get install -y net-tools
# RUN apk add --update nodejs nodejs-npm
WORKDIR /app
COPY --from=builder /app/out ./
ENV ASPNETCORE_URLS=http://+:$port

# Run the application. REPLACE the name of dll with the name of the dll produced by your application
EXPOSE $port
CMD ["dotnet", "Auction.dll"]