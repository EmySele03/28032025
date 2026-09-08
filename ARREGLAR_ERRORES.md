# Arreglar errores rojos de ActivityMainBinding

Los errores aparecen porque aún no se generó `ActivityMainBinding`.

## Paso 1 — Abrir Gradle del módulo app

Panel izquierdo → **Gradle Scripts** → **`build.gradle.kts` (Module :app)**

(Si tu proyecto usa Groovy, el archivo se llama `build.gradle` (Module :app).)

## Paso 2 — Activar View Binding

Dentro de `android { ... }` agrega (o verifica que exista):

```kotlin
buildFeatures {
    viewBinding = true
}
```

Ejemplo de ubicación:

```kotlin
android {
    namespace = "com.example.condori"
    // ...

    buildFeatures {
        viewBinding = true
    }
}
```

Si es `build.gradle` (Groovy):

```groovy
buildFeatures {
    viewBinding true
}
```

## Paso 3 — Sync

Arriba aparece **Sync Now** → haz clic.  
O: **File → Sync Project with Gradle Files**.

## Paso 4 — Rebuild

**Build → Rebuild Project**.

Cuando termine, `ActivityMainBinding` y `binding.etNombre` etc. dejan de estar en rojo.

## Sobre "No target device found"

Eso es aparte: no hay emulador/dispositivo.

1. **Device Manager** (ícono del teléfono) → **Create Device** → Pixel → Next → Download un system image → Finish  
2. Arranca el emulador (▶)  
3. Vuelve a Run ▶ en la app
