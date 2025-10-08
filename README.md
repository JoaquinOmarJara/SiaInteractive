# Full-Stack Product Management App (C# .NET + Angular)

Este es un proyecto full-stack diseñado para gestionar un catálogo de productos y sus categorías. La solución incluye un backend robusto construido con C# y .NET, y un frontend moderno e interactivo desarrollado con Angular.

---
## 🏛️ Arquitectura y Tecnologías

La aplicación sigue principios de diseño modernos y una arquitectura en capas para asegurar que sea mantenible, escalable y testeable.

### ⚙️ Backend (.NET / C#)

* **Framework**: ASP.NET Core Web API
* **Lenguaje**: C#
* **Arquitectura**: **N-Layer** (API, Application, Domain, Infrastructure)
* **Principios**: **SOLID** para un código desacoplado y mantenible.
* **Base de Datos**: **SQL Server** con **Entity Framework Core** (enfoque Code-First).
* **Patrones de Diseño**:
    * **Repositorio y Unidad de Trabajo (Repository & Unit of Work)** para abstraer el acceso a datos.
    * **Inyección de Dependencias (DI)** utilizada en toda la aplicación.
* **Features Clave**:
    * **Middleware** para gestión centralizada de excepciones.
    * Uso de **DTOs (Data Transfer Objects)** y **AutoMapper** para comunicar la API y la lógica de negocio.

### 🎨 Frontend (Angular)

* **Framework**: **Angular**
* **Arquitectura**: **Standalone Components**, el enfoque moderno sin `NgModules`.
* **Comunicación**: Servicios con **HttpClient** para consumir la API RESTful.
* **Formularios**: **Reactive Forms** para un manejo robusto de la entrada de datos.
* **Estilos**: **SCSS** para estilos avanzados, mantenibles y visualmente atractivos.
* **Enrutamiento**: Configuración de rutas para la navegación en una Single-Page Application (SPA).

### 🧪 Testing

* **Backend**: Pruebas unitarias para la capa de Lógica de Negocio (`Application`).
* **Herramientas**: **xUnit** (framework de testing), **Moq** (para mocks de dependencias) y **FluentAssertions** (para validaciones legibles).

---
## 🚀 Cómo Empezar

Sigue estos pasos para configurar y ejecutar el proyecto en tu entorno local.

### Requisitos Previos

* [.NET SDK](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) (Express o Developer)
* [Node.js y npm](https://nodejs.org/)
* [Angular CLI](https://angular.io/cli): `npm install -g @angular/cli`

### Configuración del Backend

1.  **Clona el repositorio:**
    ```bash
    git clone [https://URL-DE-TU-REPO.git](https://URL-DE-TU-REPO.git)
    cd ruta/al/backend
    ```
2.  **Configura la base de datos:**
    * Abre el archivo `appsettings.json` en el proyecto API.
    * Modifica la `ConnectionString` para que apunte a tu instancia de SQL Server.
3.  **Aplica las migraciones:**
    * Este comando creará la base de datos y las tablas según el modelo Code-First.
    ```bash
    dotnet ef database update
    ```
4.  **Ejecuta la API:**
    ```bash
    dotnet run
    ```
    La API estará disponible en la URL especificada (ej. `https://localhost:7123`).

### Configuración del Frontend

1.  **Navega a la carpeta del frontend:**
    ```bash
    cd ruta/al/frontend
    ```
2.  **Instala las dependencias:**
    ```bash
    npm install
    ```
3.  **Configura la URL de la API:**
    * Abre el archivo `src/environments/environment.ts`.
    * Asegúrate de que la propiedad `apiUrl` apunte a la URL de tu backend.
4.  **Ejecuta la aplicación Angular:**
    ```bash
    ng serve
    ```
    La aplicación estará disponible en `http://localhost:4200`.

---
## 📦 Despliegue

La aplicación está preparada para ser desplegada en **Azure**.

* **Backend (API)**: Se despliega en un **Azure App Service**.
* **Frontend (Angular)**: También se despliega en un **Azure App Service**, utilizando un archivo `web.config` para manejar correctamente el enrutamiento de la SPA.

---
## 📋 `.gitignore`

El repositorio incluye un archivo `.gitignore` configurado para ignorar las carpetas de compilación (`[Bb]in/`, `[Oo]bj/`), dependencias (`node_modules`) y otros archivos generados para mantener el control de versiones limpio.

---
# Examen teorico

# Escalabilidad y Diseño Arquitectónico 🏗️

A continuación, se presentan una serie de respuestas teóricas relacionadas con la escalabilidad, distribución de carga, y diseño de sistemas de alto volumen.

### 1. Diseño de API Escalable 🚀

**Pregunta:** ¿Cómo diseñarías una API capaz de responder a miles de dispositivos que consultan cada pocos minutos, manteniendo baja latencia y alta disponibilidad?

**Solución:** La clave es una arquitectura distribuida y sin estado (*stateless*), distribuyendo la carga horizontalmente y minimizando el tiempo de respuesta en cada capa.

#### Arquitectura Propuesta

1.  **Balanceador de Carga (Load Balancer)**
    * **Función:** Es el punto de entrada que distribuye el tráfico entre múltiples servidores de aplicación para evitar sobrecargas.
    * **Técnicas:** Algoritmos como `Round Robin` o `Least Connections`.
    * **Pros:** ✅ Alta disponibilidad, facilita la escalabilidad horizontal.
    * **Contras:** ❌ Puede ser un punto único de fallo si no se configura en un clúster de alta disponibilidad.

2.  **Servidores de Aplicación Stateless**
    * **Función:** Ejecutan la lógica de la API sin guardar información de sesión. El estado se externaliza a una base de datos o caché.
    * **Técnicas:** Uso de contenedores (`Docker`) y orquestadores (`Kubernetes`) para escalar automáticamente el número de instancias.
    * **Pros:** ✅ Escalabilidad elástica y sencilla. Cualquier servidor puede procesar cualquier petición.
    * **Contras:** ❌ Requiere una solución de almacenamiento centralizado para los estados.

3.  **Capa de Caché Distribuida**
    * **Función:** Almacenar temporalmente los datos más frecuentes para reducir la latencia y la carga sobre la base de datos.
    * **Técnicas:** Implementar un sistema de caché en memoria como **Redis** o **Memcached** con una estrategia `cache-aside`.
    * **Pros:** ✅ Respuestas casi instantáneas para datos cacheados y menor carga en la base de datos.
    * **Contras:** ❌ Complejidad en la invalidación de la caché y riesgo de datos no actualizados (*stale data*).

4.  **Base de Datos Escalable**
    * **Función:** Gestionar el almacenamiento y la recuperación de datos de forma eficiente a gran escala.
    * **Técnicas:**
        * **Replicación (Read Replicas):** Copias de la base de datos para distribuir las operaciones de lectura.
        * **Sharding (Fragmentación):** División horizontal de la base de datos en múltiples servidores (*shards*).
        * **Bases de Datos NoSQL:** Usar soluciones como **Cassandra** o **DynamoDB**, diseñadas para escalar horizontalmente.
    * **Pros:** ✅ Capacidad para manejar grandes volúmenes de datos y operaciones.
    * **Contras:** ❌ Aumenta la complejidad operativa y de las consultas (especialmente con *sharding*).

***

### 2. Distribución Masiva de Contenido 🌎

**Pregunta:** ¿Qué estrategias implementarías para asegurar que miles de dispositivos descarguen contenido nuevo sin generar cuellos de botella?

**Solución:** La estrategia principal es acercar el contenido al usuario final y gestionar de forma inteligente cómo y cuándo se solicita.

#### Estrategias de Implementación

1.  **Red de Distribución de Contenido (CDN - Content Delivery Network)**
    * **Función:** Es una red de servidores distribuidos geográficamente que almacenan copias del contenido para entregarlo desde la ubicación más cercana al usuario.
    * **Técnica:** Cuando un dispositivo solicita contenido, la petición se enruta al *Edge Server* más cercano de la CDN.
    * **Pros:** ✅ Latencia muy baja, reduce la carga en los servidores de origen, alta disponibilidad.
    * **Contras:** ❌ Costo asociado al servicio y complejidad en la invalidación del caché.
    * **Proveedores:** Amazon CloudFront, Cloudflare, Akamai.

    

2.  **Actualizaciones Delta (Delta Updates)**
    * **Función:** Enviar solo los cambios (el "delta") desde la última versión en lugar del paquete completo.
    * **Técnica:** Se genera un parche con las diferencias que el cliente aplica localmente para reconstruir el contenido actualizado.
    * **Pros:** ✅ Reduce significativamente el tamaño de las descargas y ahorra ancho de banda.
    * **Contras:** ❌ Añade complejidad en el servidor (generación de parches) y en el cliente (aplicación).

3.  **Despliegue Gradual y Aleatorio (Staggered Rollout)**
    * **Función:** Evitar que todos los dispositivos intenten descargar el contenido al mismo tiempo.
    * **Técnica:** Liberar las actualizaciones por lotes o introducir un retardo aleatorio (*jitter*) en los clientes para que la comprobación de nuevas versiones se distribuya en el tiempo.
    * **Pros:** ✅ Suaviza los picos de tráfico, evitando la sobrecarga de la infraestructura.
    * **Contras:** ❌ Algunos dispositivos recibirán el contenido más tarde que otros.

***

### 3. Consultas de Datos a Gran Escala para Dashboards 📊

**Pregunta:** ¿Qué mecanismos aplicarías para optimizar dashboards que consultan datos de miles de dispositivos en tiempo real sin afectar el rendimiento?

**Solución:** Se requiere una arquitectura que separe la ingesta de datos del sistema de consultas y que pre-procese la información para agilizar las visualizaciones.



#### Mecanismos de Optimización

1.  **Pipeline de Ingesta de Datos en Tiempo Real**
    * **Función:** Desacoplar la recepción de datos de los dispositivos de su procesamiento y almacenamiento.
    * **Técnica:** Utilizar un bus de mensajes como **Apache Kafka** o **AWS Kinesis**. Los dispositivos envían sus datos a un *topic*, desde donde múltiples sistemas pueden consumirlos.
    * **Pros:** ✅ Arquitectura desacoplada, escalable y tolerante a fallos.
    * **Contras:** ❌ Introduce una nueva pieza en la arquitectura que requiere gestión.

2.  **Base de Datos Analítica (OLAP)**
    * **Función:** Utilizar una base de datos optimizada para consultas de agregación complejas, típicas de los dashboards.
    * **Técnicas:**
        * **Bases de Datos Columnares (ej. Google BigQuery, ClickHouse):** Ideales para `SUM`, `AVG`, `COUNT` sobre grandes volúmenes de datos.
        * **Bases de Datos de Series Temporales (ej. InfluxDB, Prometheus):** Especializadas en datos con marca de tiempo.
    * **Pros:** ✅ Consultas analíticas miles de veces más rápidas que en bases de datos tradicionales (OLTP).
    * **Contras:** ❌ No son eficientes para transacciones o actualizaciones de registros individuales.

3.  **Pre-agregación y Vistas Materializadas**
    * **Función:** Realizar los cálculos complejos de antemano en lugar de hacerlo en tiempo real con cada consulta.
    * **Técnica:** Un proceso en segundo plano lee los datos crudos, calcula agregados (por minuto, hora, día) y los guarda en tablas optimizadas para que el dashboard las consulte.
    * **Pros:** ✅ Consultas extremadamente rápidas y menor carga computacional en el momento de la visualización.
    * **Contras:** ❌ Introduce una pequeña latencia (datos "casi" en tiempo real) y aumenta el costo de almacenamiento.

4.  **Push vs. Pull para la Actualización del Frontend**
    * **Función:** Enviar actualizaciones al dashboard en tiempo real sin que este tenga que solicitarlas constantemente.
    * **Técnica:** Usar **WebSockets** o **Server-Sent Events (SSE)** para que el servidor "empuje" los nuevos datos al cliente.
    * **Pros:** ✅ Reduce el tráfico de red y proporciona una experiencia de usuario fluida y en tiempo real.
    * **Contras:** ❌ Consume más recursos en el servidor al mantener conexiones abiertas con muchos clientes.
