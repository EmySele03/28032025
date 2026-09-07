# Práctica N.º 1 — Aplicación Básica Android con View Binding e Interfaz XML

## Cómo abrir el proyecto

1. Abre **Android Studio**.
2. **File → Open** y selecciona esta carpeta.
3. Espera a que Gradle sincronice.
4. Ejecuta la app en un emulador o dispositivo (API 33+).

## Requisitos cumplidos

| Requisito | Estado |
|-----------|--------|
| Empty Views Activity / Java / minSdk 33 / Kotlin DSL | ✅ |
| 2 `EditText` + 1 `Button` + 1 `TextView` | ✅ |
| Al pulsar el botón se leen los campos y se muestran en el `TextView` | ✅ |
| Raíz `ConstraintLayout` + `LinearLayout` interior | ✅ |
| View Binding activado y usado en `MainActivity` | ✅ |
| Layouts vertical y horizontal (`layout` / `layout-land`) | ✅ |
| Sin iconos ni labels obligatorios de nombre/carrera/resultado | ✅ |

## Estructura importante

- `app/src/main/java/.../MainActivity.java` — lógica con View Binding
- `app/src/main/res/layout/activity_main.xml` — vertical
- `app/src/main/res/layout-land/activity_main.xml` — horizontal
- `app/build.gradle.kts` — `viewBinding = true`, `minSdk = 33`

## Nota sobre el nombre del proyecto

La consignas pide el nombre `ApellidoNombre`. Renombra el módulo/proyecto en Android Studio si tu docente lo exige con tu apellido y nombre.
