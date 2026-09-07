package com.example.miprimeraapp;

import android.graphics.Color;
import android.os.Bundle;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.miprimeraapp.databinding.ActivityMainBinding;

/**
 * Práctica N.º 1: App básica con View Binding e interfaz XML.
 * Lee dos EditText y muestra el resultado en un TextView al pulsar el botón.
 */
public class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // View Binding: inflar el layout y asignarlo como contenido
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        binding.btnMostrar.setOnClickListener(v -> mostrarDatos());
    }

    private void mostrarDatos() {
        String campo1 = binding.etCampo1.getText().toString().trim();
        String campo2 = binding.etCampo2.getText().toString().trim();

        if (campo1.isEmpty() || campo2.isEmpty()) {
            Toast.makeText(this, "Completa ambos campos", Toast.LENGTH_SHORT).show();
            return;
        }

        String mensaje = campo1 + "\n" + campo2;
        binding.tvResultado.setText(mensaje);
        binding.tvResultado.setTextColor(Color.BLACK);
    }
}
