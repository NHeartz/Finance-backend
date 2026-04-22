FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# คัดลอกไฟล์โปรเจกต์และติดตั้ง dependencies
COPY *.csproj ./
RUN dotnet restore

# คัดลอกโค้ดทั้งหมดและทำการ Build
COPY . ./
RUN dotnet publish -c Release -o out

# สร้าง Image สำหรับรันแอปพลิเคชัน
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# ระบุชื่อไฟล์ DLL (อ้างอิงจากชื่อ Namespace ของคุณ)
ENTRYPOINT ["dotnet", "Finance.Api.dll"]