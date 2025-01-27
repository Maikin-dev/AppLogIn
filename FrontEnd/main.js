    // Validación de Bootstrap
    (function () {
        'use strict';
        const forms = document.querySelectorAll('.needs-validation');
  
        Array.from(forms).forEach(function (form) {
          form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
              event.preventDefault();
              event.stopPropagation();
            }
            form.classList.add('was-validated');
          }, false);
        });
      })();

console.log(fetch('https://localhost:7240/swagger/index.html'))