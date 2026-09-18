# Inmobiliaria-DeborahGomez

> Sistema para la gestión de alquileres temporarios de propiedades inmuebles que realiza una agencia inmobiliaria.

---

## 👥 Integrantes del Grupo

* **Dario Godoy** - *dariogodoy0896@gmail.com* - [@GodoyDario](https://github.com/GodoyDario)
* **Deborah Gomez** - *deborahgomez71@gmail.com* - [@debbieagomez](https://github.com/debbieagomez)
* **Ian Quimey Pereyra** - *0108.facultad@gmail.com* - [@necoian](https://github.com/necoian)

---

## 📐 Modelado de Datos

A continuación se presenta el esquema del modelo de datos correspondiente a la aplicación:

### Diagrama Entidad-Relación (DER) / Diagrama de Clases

![Diagrama del Proyecto](./docs/inmobiliaria.png)

> **Nota:** la imagen se encuentra en la carpeta `/docs` del repositorio. También se incluye el código Mermaid original más abajo, desplegable.

<details>
<summary>Ver diagrama en código Mermaid (Opcional)</summary>

```mermaid
classDiagram
    class Propietario {
        +int IdPropietario
        +string Nombre
        +string Apellido
        +string Dni
        +string Telefono
        +string Email
        +List~Inmueble~ Inmuebles
    }

    class TipoInmueble {
        +int IdTipoInmueble
        +string Nombre
    }

    class Inmueble {
        +int IdInmueble
        +string Direccion
        +int Cupo
        +decimal Latitud
        +decimal Longitud
        +decimal PrecioPorDia
        +decimal PorcentajeSenia
        +bool Disponible
        +int PropietarioId
        +int TipoInmuebleId
        +List~ImagenInmueble~ Imagenes
    }

    class ImagenInmueble {
        +int IdImagenInmueble
        +int InmuebleId
        +string Url
        +bool EsPortada
    }

    class Inquilino {
        +int IdInquilino
        +string Dni
        +string NombreCompleto
        +string Telefono
        +string Email
    }

    class Reserva {
        +int IdReserva
        +DateTime FechaDesde
        +DateTime FechaHasta
        +DateTime FechaHastaOriginal
        +decimal MontoPorDia
        +bool Finalizada
        +DateTime? FechaFinalizacionAnticipada
        +decimal? MontoMulta
        +int InmuebleId
        +int InquilinoId
        +int UsuarioCreadorId
        +int? UsuarioFinalizadorId
        +List~Pago~ Pagos
    }

    class Pago {
        +int IdPago
        +string Concepto
        +DateTime FechaPago
        +decimal Importe
        +bool Anulado
        +DateTime? FechaAnulacion
        +int ReservaId
        +int UsuarioCreadorId
        +int? UsuarioAnuladorId
    }

    class Usuario {
        +int IdUsuario
        +string Email
        +string PasswordHash
        +string Rol
        +string Avatar
    }

    class RolUsuario {
        <<enumeration>>
        Administrador
        Empleado
    }

    Propietario "1" --> "0..*" Inmueble : posee
    TipoInmueble "1" --> "0..*" Inmueble : clasifica
    Inmueble "1" --> "0..*" ImagenInmueble : contiene
    Inmueble "1" --> "0..*" Reserva : es_reservado_en
    Inquilino "1" --> "0..*" Reserva : realiza
    Reserva "1" --> "0..*" Pago : tiene
    Usuario "1" --> "0..*" Reserva : crea
    Usuario "1" --> "0..*" Reserva : finaliza
    Usuario "1" --> "0..*" Pago : registra
    Usuario "1" --> "0..*" Pago : anula
    Usuario --> RolUsuario : tiene
```

</details>

## Credenciales de prueba

### Administrador

- Usuario: `admin@gmail.com`
- Contraseña: `123456`

### Empleado

- Usuario: `empleado@gmail.com`
- Contraseña: `123456`

### Pasos para ingresar a MySQL Workbench e inicializar localmente la base de datos:

1. Abrir **MySQL Workbench** y seleccionar la conexión correspondiente al servidor MySQL local.

2. Ir a:

   `File -> Open SQL Script...`

3. Seleccionar el archivo:

   `Database/DBInmobiliaria_DeborahGomez.sql`

4. Cuando les abra la ventana con la query colocar en ejecutar todo.

5. Actualizar la sección **Schemas -> Refresh All** y verificar que exista la base:

   `dbinmobiliaria_deborahgomez`

6. Dirigirse a:

   `Server -> Data Import`

7. Seleccionar **Import from Self-Contained File** y elegir:

   `Database/Seed.sql`

8. Seleccionar como esquema de destino:

   `dbinmobiliaria_deborahgomez`

**Nota:** Si no aparece el esquema de destino, abrir nuevamente la pestaña `Server -> Data Import`

9. Presionar **Start Import** y verificar que se hayan cargado los datos de prueba.

10. Abrir una terminal posicionándose sobre la carpeta raíz del proyecto:

    `Inmobiliaria-DeborahGomez`

11. Inicializar User Secrets:

    ```bash
    dotnet user-secrets init
    ```

12. Configurar la conexión a MySQL reemplazando `TU-CONTRASEÑA` por la contraseña correspondiente:

    ```bash
    dotnet user-secrets set "ConnectionStrings:MySqlConnection" "Server=localhost;Port=3306;Database=dbinmobiliaria_deborahgomez;Uid=root;Pwd=TU-CONTRASEÑA;"
    ```

13. Verificar la configuración:

    ```bash
    dotnet user-secrets list
    ```

14. Ejecutar la aplicación:

    ```bash
    dotnet run
    ```
