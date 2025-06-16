document.addEventListener("DOMContentLoaded", function () {
  console.log("Página carregada, inicializando");

  // Inicialização das abas
  document.querySelectorAll(".tab-content").forEach(tab => {
    tab.style.display = "none";
  });
  const tab1 = document.getElementById("tab1");
  if (tab1) tab1.style.display = "block";
  const btn1 = document.querySelector('.tab-button[onclick*="tab1"]');
  if (btn1) btn1.classList.add("active");

  const btnCalcular = document.getElementById("btnCalcular");
  const resultado = document.getElementById("resultado");
  const tabela = document.getElementById("tabelaResultados").getElementsByTagName("tbody")[0];

  btnCalcular.addEventListener("click", async () => {
    console.log("Botão Calcular clicado");

    const variaveis = {};
    const parametros = {};

    document.querySelectorAll('input[type="number"]').forEach(input => {
      const nome = input.name;
      if (nome.startsWith("variavel_")) {
        variaveis[nome.replace("variavel_", "")] = parseFloat(input.value);
      } else if (nome.startsWith("parametro_")) {
        parametros[nome.replace("parametro_", "")] = parseFloat(input.value);
      }
    });

    const data = { variaveis, parametros };
    console.log("📤 Dados enviados para a API:", JSON.stringify(data, null, 2));

    try {
      const response = await fetch("/ui/calculate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
      });

      if (!response.ok) throw new Error(`Erro HTTP: ${response.status}`);

      const result = await response.json();
      console.log("📥 Resposta completa da API:", result);
      console.log("🔍 Resultados brutos:", JSON.stringify(result.resultados, null, 2));

      resultado.value = result.codigo_longarina || "";

      tabela.innerHTML = "";
      result.resultados.forEach((item) => {
        const row = tabela.insertRow();

        const show = v => (typeof v === "number" ? v.toFixed(4) : v ?? "–");

        row.innerHTML = `
          <td>${item.perfil ?? "–"}</td>
          <td>${show(item.peso_linear)}</td>
          <td>${show(item.flt)}</td>
          <td>${show(item.flecha)}</td>
          <td>${item.situacao ?? "–"}</td>
          <td>${show(item.m_drlflt)}</td>
          <td>${show(item.m_drdelta)}</td>
        `;
      });
 

    } catch (error) {
      console.error("❌ Erro ao calcular:", error);
      alert("Erro ao calcular. Veja o console para detalhes.");
    }
  });
});

function openTab(tabId, buttonElement) {
  console.log(`Abrindo aba: ${tabId}`);
  document.querySelectorAll(".tab-content").forEach(tab => {
    tab.style.display = "none";
  });
  document.querySelectorAll(".tab-button").forEach(btn => {
    btn.classList.remove("active");
  });
  const selectedTab = document.getElementById(tabId);
  if (selectedTab) selectedTab.style.display = "block";
  if (buttonElement) buttonElement.classList.add("active");
}


