package com.example.condoriflores;

import android.os.Bundle;
import android.view.View;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.condoriflores.databinding.ActivityMainBinding;

public class MainActivity extends AppCompatActivity {

    // Variable para View Binding
    private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);

        // Instanciar el objeto binding:
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        View view = binding.getRoot();
        setContentView(view);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        // Al pulsar el botón: leer los 2 EditText y mostrarlos en lblResult
        binding.btnMostrar.setOnClickListener(v -> {
            String nombre = binding.idNombreText.getText().toString().trim();
            String carrera = binding.idCarreraText.getText().toString().trim();
            binding.lblResult.setText(nombre + "\n" + carrera);
        });

    }//fin metodo onCreate

}//fin de la clase MainActivity
