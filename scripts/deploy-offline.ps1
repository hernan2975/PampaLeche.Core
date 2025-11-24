# deploy-offline.ps1
# Despliegue offline para entornos de planta láctea en La Pampa
# Ejecutar desde máquina con .NET 8 SDK (una sola vez); copiar salida a dispositivo en campo

$OutputDir = "deploy"
$Project = "src\PampaLeche.Presentation\Cli"

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
}

Write-Host "📦 Compilando para entorno offline (win-x64, linux-x64, linux-arm64)..."
Write-Host ""

# Windows x64
dotnet publish $Project -c Release -r win-x64 --self-contained true -o "$OutputDir\win-x64"
Write-Host "✅ win-x64 listo"

# Linux x64 (para PC/Notebook en planta)
dotnet publish $Project -c Release -r linux-x64 --self-contained true -o "$OutputDir\linux-x64"
Write-Host "✅ linux-x64 listo"

# Linux ARM64 (para Raspberry Pi 4/5 en tanque)
dotnet publish $Project -c Release -r linux-arm64 --self-contained true -o "$OutputDir\linux-arm64"
Write-Host "✅ linux-arm64 listo"

# Incluir script de reporte y guía rápida
Copy-Item "scripts\generate-senasa-report.ps1" "$OutputDir\" -Force
@"
Guía Rápida — PampaLeche.Core (La Pampa)
-----------------------------------------
1. Copie la carpeta correspondiente a su dispositivo (ej: linux-arm64)
2. Ejecute:
   ./PampaLeche.Presentation   # Linux
   PampaLeche.Presentation.exe # Windows
3. Al finalizar, ejecute:
   ./generate-senasa-report.ps1
4. Copie la carpeta 'reports' a unidad removible.
"@ | Set-Content "$OutputDir\GUÍA_RÁPIDA.txt" -Encoding UTF8

Compress-Archive -Path "$OutputDir\*" -DestinationPath "$OutputDir\pam-leche-deploy.zip" -Force
Write-Host ""
Write-Host "📦 Paquete listo: $OutputDir\pam-leche-deploy.zip"
Write-Host "→ Copie este ZIP a dispositivo en campo (sin internet)."
