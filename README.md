# 📚 BookRadar

Aplicación web **ASP.NET Core MVC (.NET 8)** que permite buscar libros por autor usando la API pública de **Open Library**, mostrar los resultados en una tabla Razor fuertemente tipada y guardar un historial en **SQL Server**.

> Este proyecto crea automáticamente la base de datos, tablas y procedimientos almacenados al iniciar la aplicación, por lo que **no es necesario ejecutar scripts manualmente**.

---

## 🚀 Características principales

- **Búsqueda de libros** por autor usando la API REST de Open Library.
- **Visualización de resultados** en una tabla Razor fuertemente tipada.
- **Persistencia automática en SQL Server** del historial de búsquedas.
- **Historial de búsquedas** consultado desde la base de datos.
- **Prevención de duplicados**: no guarda la misma búsqueda si se realiza en menos de 1 minuto.
- **Maquetado con Bootstrap 5** para interfaz responsiva.
- **Validaciones en frontend** con JavaScript.

---

## 🛠 Requisitos previos

- **.NET 8 SDK**: [Descargar](https://dotnet.microsoft.com/download)
- **SQL Server** (LocalDB o instancia)
- **Visual Studio 2022** o VS Code con extensiones para .NET
- **Conexión a internet** para consumir la API pública

---

## ⚙️ Pasos para ejecutar el proyecto

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/lordimpi/BookRadar.git
   cd BookRadar
   ```

2. **Configurar la cadena de conexión**
   - Edita `appsettings.json` y actualiza la cadena de conexión a tu entorno SQL Server:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=BookRadarDB;Trusted_Connection=True;TrustServerCertificate=True"
     }
     ```

3. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

4. **Ejecutar la aplicación**

   **Opción A — Usando .NET CLI (VS Code o terminal)**
   ```bash
   dotnet run --project BookRadar
   ```

   **Opción B — Usando Visual Studio 2022**
   - Abrir el archivo de solución `.sln` en Visual Studio 2022.
   - Seleccionar el proyecto **BookRadar** como proyecto de inicio.
   - En la barra de herramientas, elegir el perfil **IIS Express** o **BookRadar**.
   - Presionar **F5** para ejecutar con depuración o **Ctrl + F5** para ejecutar sin depuración.

5. **Abrir en el navegador**
   - Ir a: [https://localhost:5001](https://localhost:5001) o [http://localhost:5000](http://localhost:5000) según la configuración.

> La primera ejecución creará automáticamente la base de datos, tablas e insertará los stored procedures necesarios.

---

## 🖼 Uso

1. Ingresar el nombre de un autor en el formulario de búsqueda.
2. Ver los resultados obtenidos desde Open Library.
3. Los resultados se guardarán en el historial con fecha de consulta.
4. El historial se muestra en una tabla independiente.

---

## 🎨 Decisiones de diseño

- **Bootstrap 5** para una interfaz limpia y responsiva.
- **Tablas claras** con encabezados visibles y espaciado adecuado.
- **Validaciones simples** con JavaScript para evitar envíos vacíos.
- **Prevención de duplicados** implementada en servicio para mejorar experiencia de usuario.
- **Separación de responsabilidades** en controladores, servicios y vistas para facilitar mantenimiento.

---

## 📌 Mejoras pendientes

- Agregar **filtros avanzados** (por año, editorial).
- Incluir **autenticación de usuario** para búsquedas personalizadas.
- Mejorar **test unitarios** y cobertura de código.
- Internacionalización para soportar múltiples idiomas.

---

## 📄 Licencia

Este proyecto es de uso libre para fines académicos y de evaluación técnica.