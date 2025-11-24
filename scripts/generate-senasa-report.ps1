Write-Host "Generando reporte SENASA..."
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$report = "reports/senasa_$timestamp.csv"

New-Item -ItemType Directory -Path "reports" -Force | Out-Null

@"
batch,origin,grasa(%),densidad,acidez,estado,ubicacion
LP2025-11-24-001,LP-10045,3.2,1.031,0.16,Aceptado,-36.6167,-64.2833
"@ | Set-Content $report

Write-Host "✅ Reporte generado: $report"
