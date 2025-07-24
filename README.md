# RetoTecnico

Este proyecto está desarrollado con **.NET 9** y ofrece una solución práctica que incluye:

- Un **CRUD de usuarios** con autenticación mediante **JWT (JSON Web Tokens)** usando **nombre de usuario y contraseña**.
- Un **CRUD de medicamentos** con relación a la forma farmacéutica.
- Un módulo para la **subida de un XML específico**, extracción del UUID y consumo de un Web Service SOAP para obtener un archivo PDF en Base64.
- Arquitectura separada en backend y frontend para mantener una estructura limpia y escalable.

---

## Estructura del Proyecto

### 1. **`RetoTecnico.API` (Backend)**

- Aplicación `ASP.NET Core Web API`.
- Expone endpoints REST protegidos con autenticación JWT.
- Implementa **CRUDs** para los módulos de usuarios y medicamentos.
- Proporciona endpoint para consulta de CFDI vía Web Service SOAP externo.
- Toda la configuración sensible se gestiona a través de `appsettings.json`.

### 2. **`RetoTecnico.Web` (Frontend)**

- Aplicación web basada en `ASP.NET Core Razor Pages` con jQuery.
- Permite autenticación por nombre de usuario y contraseña.
- Interfaz para subir archivo XML (drag & drop) o ingresar manualmente UUID.
- Listados paginados con filtros para usuarios y medicamentos, con CRUD mediante modales.
- Consumo del backend vía Web API para todas las operaciones.

---

## Funcionalidades Principales

### 🔐 CRUD de Usuarios con JWT

- Registro, edición y eliminación de usuarios.
- Validación de contraseña con requisitos de seguridad (mínimo 8 caracteres, mayúsculas, minúsculas, números y caracteres especiales).
- Paginación y filtros en listados.
- Autenticación con JWT usando nombre de usuario y contraseña para proteger los endpoints.

### 📄 Lectura XML y Consulta CFDI

- Subida de XML mediante drag & drop.
- Extracción automática del UUID desde el nodo `tfd:TimbreFiscalDigital`.
- Consumo del Web Service SOAP externo con usuario y contraseña demo para obtener PDF en Base64.
- Visualización del PDF en una nueva pestaña del navegador.

---

## Configuración (`appsettings.json`)

### Backend – `RetoTecnico.API/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-1145PHU;Database=Cepdi_Prueba;User Id=cepdiprueba;Password=Sd#s4s.S425D;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "sdfsdsdfsdfsdfsdfswerhjntyhjdfgbfgbrtghrtbrtb",
    "Issuer": "https://retocepdi.com",
    "Audience": "https://retocepdi.com"
  },
  "WebServiceSettings": {
    "Usuario": "demo1@mail.com",
    "Password": "Demo123#"
  }
}
```

### Frontend – `RetoTecnico.Web/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ApiSettings": {
    "BaseUrl": "http://localhost:5181"
  }
}
```

---

## 🔧 Acciones Futuras

## 🛠️ Backend (RetoTecnico.API)

- Implementar pruebas unitarias: Para asegurar el correcto funcionamiento de los componentes y facilitar refactorizaciones sin temor a romper la lógica existente.

- Implementar circuit breaker con retry + backoff: Para hacer más resiliente la comunicación con el Web Service externo ante errores intermitentes o caídas del servicio.

- Crear un ExceptionHandlingMiddleware: Para centralizar el manejo de excepciones y garantizar respuestas consistentes ante errores inesperados.

- Normalizar las respuestas del API (meta y data): Para mejorar la consistencia en el consumo del API desde el frontend o integraciones externas.

- Realizar documentación general (diagramas de componentes, etc.): Para facilitar el entendimiento del sistema por parte de otros desarrolladores o futuros mantenedores.

- Implementar endpoints de healthchecks: Para permitir monitoreo automatizado de la salud y disponibilidad del servicio, útil para producción o despliegues con contenedores.

- Mapeo adecuado de logs: Para facilitar el seguimiento de errores y trazabilidad en ambientes de desarrollo y producción.

- Documentar la API con OpenAPI/Swagger: Para ofrecer una interfaz interactiva de prueba y documentación técnica estandarizada.

- Implementar cache para información que es consumida regularmente. Para mejora del performance se podría realizar la implementación de cache para endpoints que sean altamente consumidos con los mismos parametros. La implementación podría realizarse con Redis en caso de que el despliegue fuera de multiples instancias o de lo contrario en memoria.

## 💻 Frontend (RetoTecnico.Web)

- Migrar llamadas jQuery a Fetch API: jQuery está deprecado en muchos entornos modernos; usar Fetch mejora compatibilidad y control.

- Implementar indicadores de carga (spinners): Para mejorar la experiencia del usuario mientras espera una respuesta del servidor.

- Fortalecer la autorización entre páginas: Para evitar accesos directos a páginas protegidas sin haber iniciado sesión correctamente.

- Mejorar experiencia de usuario (UX): Para que la interacción sea más intuitiva y accesible.

- Agregar validaciones más robustas en formularios: Para evitar errores comunes y mejorar la calidad de los datos enviados al backend.

- Aplicar diseño responsivo: Para asegurar que la aplicación funcione correctamente en distintos tamaños de pantalla y dispositivos.

- Considerar migración a una tecnología más robusta (React): Razor Pages se eligió por su sencillez y rapidez, pero para escalabilidad o proyectos más complejos se recomienda usar frameworks modernos como React.

---

## 🚀 Despliegue y DevOps

### Backend – Despliegue en Kubernetes

Para escenarios con alta concurrencia o gran número de usuarios, el backend se desplegaría en un **cluster de Kubernetes**.

**Ventajas:**

- Escalamiento horizontal automático para atender demanda variable.
- Alta disponibilidad gracias a la recuperación automática de pods.
- Despliegues sin interrupciones con rolling updates.
- Integración con monitoreo y logging para mantenimiento proactivo.

### Frontend – Opciones de Despliegue

- Desplegar en el mismo cluster de Kubernetes como servicio separado.
- Usar plataformas PaaS como Azure App Service, Google App Engine o AWS Elastic Beanstalk.
- En caso de migrar a SPA, desplegar en servicios estáticos tipo Netlify o Vercel.

### CI/CD y Control de Calidad

- Pipelines de integración y despliegue continuo para backend y frontend.
- Ejecución automática de pruebas, análisis de código y validación de seguridad con herramientas como SonarQube o Checkmarx (según presupuesto).
- Despliegue automatizado en ambientes de staging y producción.

---

## 🕒 Estimación de Entrega

La implementación de este proyecto completo se estima en 5 sprints de 2 semanas, considerando que una sola persona esté desarrollando a tiempo completo. Esta planificación busca generar un entregable funcional e incremental al finalizar cada sprint.

| Sprint       | Objetivo Principal                                                                                                                                                                 |
| ------------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Sprint 1** | ✔️ Configuración del entorno de trabajo, documentación técnica inicial y configuración de pipelines CI/CD. <br>✔️ Estructura base del backend y frontend.                          |
| **Sprint 2** | ✔️ Desarrollo completo del módulo de **usuarios** (Web API + Frontend con jQuery). <br>✔️ Validaciones de contraseña, paginación, filtros y CRUD con modales.                      |
| **Sprint 3** | ✔️ Desarrollo del módulo de **medicamentos** (Web API + Frontend con jQuery). <br>✔️ Integración con la entidad de forma farmacéutica.                                             |
| **Sprint 4** | ✔️ Desarrollo del módulo de **lectura de XML y consumo de Web Service SOAP**. <br>✔️ Implementación de subida Drag & Drop, extracción de UUID y apertura del PDF en nueva pestaña. |
| **Sprint 5** | ✔️ Implementación de pruebas unitarias en backend. <br>✔️ Integración de validaciones extra, refactor, mejora de manejo de errores y despliegue.                                   |
