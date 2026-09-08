# Guía — CondoriFlores (IDs de tu interfaz nueva)

Tu Component Tree debe quedar así (como en tu Design):

```
ConstraintLayout (main)
 └── LinearLayout (vertical)
      ├── TextView          → "Mi primera App"
      ├── LinearLayout (vertical)
      │    ├── EditText     → id: etNombre
      │    └── EditText     → id: etCarrera
      ├── Button            → id: btnMostrar   ("Mostrar Datos")
      └── TextView          → id: tvResult
```

---

## IDs importantes (View Binding)

| Control   | ID en XML   | En Java                  |
|-----------|-------------|--------------------------|
| Nombre    | `etNombre`  | `binding.etNombre`       |
| Carrera   | `etCarrera` | `binding.etCarrera`      |
| Botón     | `btnMostrar`| `binding.btnMostrar`     |
| Resultado | `tvResult`  | `binding.tvResult`       |

---

## Código Java (simple)

En `MainActivity.java`, con View Binding ya activado:

```java
binding.btnMostrar.setOnClickListener(v -> {
    String nombre = binding.etNombre.getText().toString().trim();
    String carrera = binding.etCarrera.getText().toString().trim();
    binding.tvResult.setText(nombre + "\n" + carrera);
});
```

Al pulsar **Mostrar Datos**, se leen los dos campos y se muestran en `tvResult` (nombre en una línea, carrera en la siguiente).

---

## Checklist de la práctica

- [x] Empty Views Activity, Java, minSdk 33, Gradle Kotlin DSL
- [x] View Binding ON (`buildFeatures { viewBinding = true }`)
- [x] ConstraintLayout + LinearLayout
- [x] ≥ 2 EditText, 1 Button, 1 TextView
- [x] Layout vertical + horizontal (`layout` y `layout-land`)
- [x] Sin iconos ni labels "Nombre:" / "Carrera:" / "Resultado:"
