async function ObtenerRubros() {

  var modal = bootstrap.Modal.getOrCreateInstance(
    document.getElementById('modalRubro')
  );

  modal.hide();

  const respuesta = await authFetch("/rubros");

  const resultado = await respuesta.json();

  LimpiarModal();



  const bodyRubros = document.getElementById("tbody-rubros");
  bodyRubros.innerHTML = "";

  resultado.datos.forEach((rubro) => {
    const tr = document.createElement("tr");

    tr.innerHTML = `
            <td>${rubro.descripcion} (ROL: ${resultado.rol})</td>
            <td class="text-center columnaBtn">
 <button class="btn btn-editar" onclick="AbrirModalEditar(${rubro.rubroID})">
        <i class="fa-solid fa-pen"></i>       
    </button>
            </td>
            <td class="text-center columnaBtn">
                <button class="btn btn-eliminar" onclick="Eliminar(${rubro.rubroID})">
                 <i class="fa-solid fa-trash"></i>
                 </button>
            </td>
        `;

    bodyRubros.appendChild(tr);
  });
}

function validarCamposRequeridos(contenedor) { //funcion que valida que los campos requeridos no esten vacios, recive por parametro el form correspondiente y hace las verificaciones
  let valido = true;

  const inputs = contenedor.querySelectorAll(".input-requerido");

  inputs.forEach(input => {
    const error = input.nextElementSibling;

    if (input.value.trim() === "") {
      error.style.display = "block";
      valido = false;
    } else {
      error.style.display = "none";
    }
  });

  return valido;
}

async function AbrirModalEditar(id) {

  try {
    const respuesta = await authFetch("/Rubros/" + id);

    if (!respuesta.ok) {
      throw new Error("No se pudo obtener el dato");
    }

    const resultado = await respuesta.json();
    document.getElementById("titulo-modal").textContent = "EDITAR RUBRO";
    document.getElementById("rubroID").value = resultado.datos.rubroID;
    document.getElementById("rubroNombre").value = resultado.datos.descripcion;

    var modal = bootstrap.Modal.getOrCreateInstance(
      document.getElementById('modalRubro')
    );

    modal.show();

  } catch (error) {
    console.error("Error editar:", error);
  }
}

async function Guardar() {

  //rubro id puede ser 0 o distinto de 0
  const rubroID = document.getElementById("rubroID").value;
  //tambien buscamos la descripcion
  const descripcion = document.getElementById("rubroNombre").value.trim();

  //con eso armamos el objeto para pasar a la api
  const rubro = {
    rubroID: rubroID,
    descripcion: descripcion
  };

  //console.log(rubro);
  //verifico que el usuario tenga escrito una descripcion
  if (descripcion != "") {
    //pregunto si rubro es mayor a 0 
    if (rubroID > 0) {
      const res = await authFetch(`/rubros/${rubroID}`, {
        method: "PUT",
        body: JSON.stringify(rubro)
      });
    }
    else {
      const res = await authFetch(`/Rubros`, {
        method: "POST",
        body: JSON.stringify(rubro)
      });

      let resultado = await res.json();
      if(!resultado.esExitoso){
        alert(resultado.mensaje);
      }

    }

    ObtenerRubros();
  }

}


async function Eliminar(rubroID) {

  try {
 
    const respuesta = await authFetch(`/rubros/${rubroID}`, {
        method: "DELETE"
      });

    if (!respuesta.ok) {
      throw new Error("No se pudo obtener el dato con id: " + id);
    }

    ObtenerRubros();

  } catch (error) {
    console.error("Error ELIMINAR:", error);
  }
}

//si se llama esta funcion es para resetar el modal y permitir cargar un nuevo registro
async function LimpiarModal() {
  document.getElementById("rubroID").value = 0;
  document.getElementById("rubroNombre").value = "";
  document.getElementById("titulo-modal").textContent = "CREAR RUBRO";
}

ObtenerRubros();
