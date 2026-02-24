# LogisticIntegration API - Acceso Local

## ✅ Estado del Servidor

El API está corriendo en:
- **HTTPS:** https://localhost:5001
- **HTTP:** http://localhost:5000
- **Perfil activo:** HTTPS

## 📋 Acceso a Swagger UI

### Opción 1: HTTP (Sin certificado)
```
http://localhost:5000/swagger
```

### Opción 2: HTTPS (Requiere ignorar errores de certificado)
```
https://localhost:5001/swagger
```

⚠️ **Nota de Certificado:** El certificado de desarrollo de ASP.NET Core no está instalado como de confianza. Esto es normal en desarrollo. Los navegadores mostrarán una advertencia de seguridad que puedes ignorar.

## 🧪 Probar el Endpoint

### Endpoint de Cálculo de Liquidación

**URL:**
```
POST http://localhost:5000/api/settlement/{guid}/calculate
```

**Ejemplo con curl:**
```bash
curl -X POST http://localhost:5000/api/settlement/550e8400-e29b-41d4-a716-446655440000/calculate
```

**Ejemplo con PowerShell:**
```powershell
$settlementId = "550e8400-e29b-41d4-a716-446655440000"
Invoke-RestMethod -Uri "http://localhost:5000/api/settlement/$settlementId/calculate" `
  -Method Post -ContentType "application/json"
```

## 📝 Logs del Servidor

El servidor está registrando todas las solicitudes. Ejemplo de salida:
```
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/1.1 GET https://localhost:5001/swagger
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished HTTP/1.1 GET https://localhost:5001/swagger 301
```

## 🛠️ Configuración

### Puerto HTTPS (5001)
Definido en `launchSettings.json` perfil "https"

### Puerto HTTP (5000)
Definido en `launchSettings.json` perfil "http"

## 🔍 Troubleshooting

### Si no puedes acceder al servidor:

1. **Verifica que el API está corriendo:**
   ```powershell
   Get-Process dotnet
   ```

2. **Verifica que el puerto está en uso:**
   ```powershell
   netstat -ano | findstr ":5001"
   ```

3. **Reinicia el API:**
   ```powershell
   taskkill /F /IM dotnet.exe
   dotnet run --project C:\Dev\Code\LogisticV2\LogisticIntegration.Api\LogisticIntegration.Api.csproj --launch-profile https
   ```

## 📚 Rutas Disponibles

| Método | Ruta | Descripción |
|--------|------|---|
| POST | `/api/settlement/{id}/calculate` | Calcula la liquidación del viaje |

### Respuestas

**200 OK (Éxito):**
```json
{
  "success": true,
  "message": "Trip settlement calculated successfully."
}
```

**404 Not Found (Liquidación no existe):**
```json
{
  "error": "Settlement with ID 'xxx' was not found."
}
```

**400 Bad Request (Error general):**
```json
{
  "error": "Error message"
}
```

---

**Levantar API:** El servidor está activo y listo para recibir solicitudes.
