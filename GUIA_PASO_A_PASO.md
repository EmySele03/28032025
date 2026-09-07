# Guía paso a paso — Interfaz (CondoriFlores)

Partes desde lo que ya tienes: proyecto **CondoriFlores**, `activity_main.xml` con Hello World, y `MainActivity.java` con View Binding.

---

## Dónde está cada cosa

| Qué | Dónde en Android Studio (panel Project) |
|-----|------------------------------------------|
| Interfaz vertical | `app → res → layout → activity_main.xml` |
| Interfaz horizontal | `app → res → layout-land → activity_main.xml` (si no existe, créala) |
| Lógica Java | `app → java → com.example.condoriflores → MainActivity.java` |
| View Binding ON | `app → build.gradle.kts (:app)` → `buildFeatures { viewBinding = true }` |
| Textos (hints, título) | `app → res → values → strings.xml` |

---

## PASO 1 — Abrir el diseño

1. En el panel izquierdo abre: **`app → res → layout → activity_main.xml`**
2. Arriba del editor elige **Code** (o Split). Design sirve para ver, pero pegar el XML es más rápido y evita errores de constraints.
3. Borra el `TextView` de **"Hello World!"** (en Component Tree: clic derecho → Delete, o bórralo en el XML).

Tu raíz ya debe ser un **`ConstraintLayout`** con `android:id="@+id/main"`. Eso se queda.

---

## PASO 2 — Meter el LinearLayout (obligatorio)

Dentro del `ConstraintLayout`, agrega un **`LinearLayout` vertical**. Ahí irán todos los controles.

En Design: Palette → Layouts → **LinearLayout (vertical)** → suéltalo dentro de `main`.

En Attributes (derecha), pon:

- **id:** `linearContenedor`
- **orientation:** `vertical`
- constraints: izquierda, derecha y arriba al parent

---

## PASO 3 — Agregar los controles (en orden, dentro del LinearLayout)

Arrástralos **dentro** de `linearContenedor` (mira el Component Tree). Orden de arriba a abajo:

1. **TextView** → id `tvTitulo` → text: `Mi Primera App`
2. **TextView** → id `tvSubtitulo` → text: `Ingrese sus datos`
3. **Plain Text (EditText)** → id `etCampo1` → hint: `Escribe tu nombre`
4. **Plain Text (EditText)** → id `etCampo2` → hint: `Escribe tu carrera`
5. **Button** → id `btnMostrar` → text: `Mostrar Datos`
6. **TextView** → id `tvResultado` → text: `Los datos aparecerán aquí`

No hace falta iconos ni labels “Nombre:” / “Carrera:” / “Resultado:”.

IDs importantes (View Binding los usa así):

- `etCampo1`, `etCampo2`, `btnMostrar`, `tvResultado`

---

## PASO 4 — Pegar el XML listo (recomendado)

Si prefieres no pelear con el Design, en `activity_main.xml` pega el contenido de:

`app/src/main/res/layout/activity_main.xml` (de este repo)

Luego Sync / Rebuild.

---

## PASO 5 — Layout horizontal

1. Clic derecho en `res` → **New → Android Resource Directory**
2. Directory name: `layout-land` → Resource type: `layout` → OK
3. Copia `activity_main.xml` dentro de `layout-land` (o pega el de este repo)
4. **Mismos IDs** que en vertical (`etCampo1`, `etCampo2`, `btnMostrar`, `tvResultado`, `main`)

Para previsualizar: en el editor de layout, el ícono de rotación del teléfono (portrait ↔ landscape).

---

## PASO 6 — Confirmar View Binding

En **`build.gradle.kts (:app)`** debe existir:

```kotlin
buildFeatures {
    viewBinding = true
}
```

Sync Now (arriba a la derecha si aparece el banner).

---

## PASO 7 — Código Java (como el tuyo)

En `MainActivity.java`, después de `setContentView(view);`, el botón:

```java
binding.btnMostrar.setOnClickListener(v -> {
    String nombre = binding.etCampo1.getText().toString().trim();
    String carrera = binding.etCampo2.getText().toString().trim();
    binding.tvResultado.setText(nombre + "\n" + carrera);
});
```

Así se conecta la interfaz con la lógica: los IDs del XML aparecen como `binding.etCampo1`, etc.

---

## PASO 8 — Probar

1. Run ▶ en un emulador
2. Escribe algo en los dos campos → **Mostrar Datos** → debe salir en el TextView de abajo
3. Rota el emulador (Ctrl+Left/Right o botón de rotación) y verifica landscape

---

## Árbol final (Component Tree vertical)

```
ConstraintLayout (main)
 └── LinearLayout (linearContenedor)
      ├── TextView (tvTitulo)
      ├── TextView (tvSubtitulo)
      ├── EditText (etCampo1)
      ├── EditText (etCampo2)
      ├── Button (btnMostrar)
      └── TextView (tvResultado)
```

Sin errores rojos en el Component Tree = bien para las capturas de la entrega.
