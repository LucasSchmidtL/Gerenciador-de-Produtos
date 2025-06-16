document.addEventListener("DOMContentLoaded", async () => {
    const produtoSelect = document.getElementById("selectProduto");
    const agrupadorSelect = document.getElementById("selectAgrupador");
    const componenteSelect = document.getElementById("selectComponente");
    const itemSelect = document.getElementById("selectItem");

    let network;
    let nodesDS = new vis.DataSet();
    let edgesDS = new vis.DataSet();

    function updateStats() {
        const totalN = nodesDS.length;
        const totalE = edgesDS.length;
        document.getElementById('totalNodes').textContent = totalN;
        document.getElementById('totalEdges').textContent = totalE;
        let maxDeg = 0, topLabel = '-';
        nodesDS.getIds().forEach(id => {
            const deg = network.getConnectedEdges(id).length;
            if (deg > maxDeg) { maxDeg = deg; topLabel = nodesDS.get(id).label; }
        });
        document.getElementById('topNode').textContent = topLabel;
    }

    function showNodeDetails(params) {
        const box = document.getElementById('floatingDetailBox');
        if (!params.nodes.length) {
            box.style.display = 'none';
            return;
        }

        const nodeId = params.nodes[0];
        const node = nodesDS.get(nodeId);
        const neighbors = network.getConnectedNodes(nodeId).map(id => nodesDS.get(id));

        const details = [
            { label: 'ID', value: node.id },
            { label: 'Nome', value: node.label },
            { label: 'Tipo', value: node.group },
            { label: 'Conexões', value: neighbors.length }
        ];

        switch (node.group) {
            case 'produto':
                details.push({ label: 'Agrupadores', value: neighbors.filter(n => n.group === 'agrupador').map(n => n.label).join(', ') || 'Nenhum' });
                break;
            case 'agrupador':
                details.push({ label: 'Componentes', value: neighbors.filter(n => n.group === 'componente').map(n => n.label).join(', ') || 'Nenhum' });
                break;
            case 'componente':
                details.push({ label: 'Itens ERP', value: neighbors.filter(n => n.group === 'item').map(n => n.label).join(', ') || 'Nenhum' });
                break;
            case 'item':
                details.push({ label: 'Perfis', value: neighbors.filter(n => n.group === 'perfil').map(n => n.label).join(', ') || 'Nenhum' });
                details.push({ label: 'Desenhos', value: neighbors.filter(n => n.group === 'desenho').map(n => n.label).join(', ') || 'Nenhum' });
                break;
            case 'perfil':
            case 'desenho':
                details.push({ label: 'Itens ERP', value: neighbors.filter(n => n.group === 'item').map(n => n.label).join(', ') || 'Nenhum' });
                break;
        }

        const html = `<strong>${node.label}</strong><ul class="list-unstyled mb-0">` +
            details.map(d => `<li><strong>${d.label}:</strong> ${d.value}</li>`).join('') +
            `</ul>`;

        const canvasPos = network.canvasToDOM(network.getPositions([nodeId])[nodeId]);

        box.innerHTML = html;
        box.style.top = `${canvasPos.y + 10}px`;
        box.style.left = `${canvasPos.x + 10}px`;
        box.style.display = 'block';
    }

    function inicializarGrafo() {
        const container = document.getElementById("network");
        const data = { nodes: nodesDS, edges: edgesDS };
        const options = {
            nodes: { shape: 'dot', size: 16 },
            edges: {
                arrows: { to: { enabled: true } },
                smooth: { type: 'continuous' }
            },
            interaction: {
                navigationButtons: true,
                hover: true
            },
            physics: { enabled: false }
        };

        network = new vis.Network(container, data, options);

        network.on("click", showNodeDetails);

        network.on("doubleClick", async function (params) {
            if (!params.nodes.length) return;
            const [prefix, id] = params.nodes[0].split("-");

            switch (prefix) {
                case "prd": await carregarGraph(`/Home/GetGraphProduto?id=${id}`); break;
                case "agr": await carregarGraph(`/Home/GetGraphAgrupador?id=${id}`); break;
                case "cmp": await carregarGraph(`/Home/GetGraphComponente?id=${id}`); break;
                case "item": await carregarGraph(`/Home/GetGraphItem?id=${id}`); break;
            }

            network.setOptions({ physics: { enabled: true, solver: "forceAtlas2Based" } });
            network.stabilize();
            updateStats();
        });

        document.getElementById("btnForceLayout").addEventListener("click", () => {
            network.setOptions({ layout: { hierarchical: false }, physics: { enabled: true, solver: 'forceAtlas2Based' } });
            network.stabilize(); updateStats();
        });

        document.getElementById("btnHierarchicalLayout").addEventListener("click", () => {
            network.setOptions({ layout: { hierarchical: true }, physics: { enabled: false } });
            network.stabilize(); updateStats();
        });

        document.getElementById("btnRadialLayout").addEventListener("click", () => {
            const levels = {};
            nodesDS.forEach(n => {
                if (!levels[n.level]) levels[n.level] = [];
                levels[n.level].push(n);
            });
            const center = { x: 0, y: 0 }, step = 150;
            Object.keys(levels).forEach(lvl => {
                const group = levels[lvl];
                const angleStep = (2 * Math.PI) / group.length;
                group.forEach((node, idx) => {
                    const angle = idx * angleStep;
                    const r = step * parseInt(lvl);
                    nodesDS.update({ id: node.id, x: center.x + Math.cos(angle) * r, y: center.y + Math.sin(angle) * r, fixed: { x: true, y: true } });
                });
            });
            network.setOptions({ physics: false });
            updateStats();
        });

        document.getElementById("nodeSearch").addEventListener("keyup", e => {
            if (e.key !== 'Enter') return;
            const term = e.target.value.toLowerCase();
            const match = nodesDS.get().find(n => n.label.toLowerCase().includes(term));
            if (match) {
                network.selectNodes([match.id]);
                network.focus(match.id, { scale: 1.5 });
            }
        });

        updateStats();
    }

    function addToGraph(nodes, edges) {
        const novosN = nodes.filter(n => !nodesDS.get(n.id));
        nodesDS.add(novosN);

        const novosE = edges.filter(e => !edgesDS.get({ filter: ed => ed.from === e.from && ed.to === e.to }).length);
        edgesDS.add(novosE);
    }

    async function carregarGraph(url) {
        const res = await fetch(url);
        const data = await res.json();
        addToGraph(data.nodes, data.edges);
        updateStats();
    }

    async function carregarSelect(url, select, label) {
        const res = await fetch(url);
        const data = await res.json();

        select.innerHTML = `<option value="">Selecione um ${label}...</option>`;
        data.forEach(item => {
            select.innerHTML += `<option value="${item.id}">${item.nome || item.erp}</option>`;
        });
        select.disabled = false;
    }

    async function carregarDetalhes(itemId) {
        const res = await fetch(`/Home/GetDetalhesItemERP?itemId=${itemId}`);
        const data = await res.json();
        const box = document.getElementById('floatingDetailBox');

        box.innerHTML = `
            <strong>${data.erp}</strong>
            <p><small>${data.descricao}</small></p>
            <p><strong>Perfis:</strong> ${[...data.perfis].join(", ")}</p>
            <p><strong>Desenhos:</strong> ${[...data.desenhos].join(", ")}</p>
            <p><strong>Relacionados:</strong> ${[...data.relacionados].join(", ")}</p>
        `;
    }

    window.toggleFullscreen = () => {
        const el = document.documentElement;
        if (!document.fullscreenElement) el.requestFullscreen();
        else document.exitFullscreen();
    };

    window.exportGraph = () => {
        html2canvas(document.getElementById('network')).then(canvas => {
            const link = document.createElement('a');
            link.href = canvas.toDataURL();
            link.download = 'grafo.png';
            link.click();
        });
    };

    window.resetZoom = () => {
        network.fit();
    };

    produtoSelect.addEventListener("change", async () => {
        const id = produtoSelect.value;
        if (!id) return;
        await carregarSelect(`/Home/GetAgrupadoresPorProduto?produtoId=${id}`, agrupadorSelect, "agrupador");
        componenteSelect.innerHTML = itemSelect.innerHTML = "";
        componenteSelect.disabled = itemSelect.disabled = true;
        await carregarGraph(`/Home/GetGraphProduto?id=${id}`);
    });

    agrupadorSelect.addEventListener("change", async () => {
        const id = agrupadorSelect.value;
        if (!id) return;
        await carregarSelect(`/Home/GetComponentesPorAgrupador?agrupadorId=${id}`, componenteSelect, "componente");
        itemSelect.innerHTML = "";
        itemSelect.disabled = true;
        await carregarGraph(`/Home/GetGraphAgrupador?id=${id}`);
    });

    componenteSelect.addEventListener("change", async () => {
        const id = componenteSelect.value;
        if (!id) return;
        await carregarSelect(`/Home/GetItensPorComponente?componenteId=${id}`, itemSelect, "item");
        await carregarGraph(`/Home/GetGraphComponente?id=${id}`);
    });

    itemSelect.addEventListener("change", async () => {
        const id = itemSelect.value;
        if (!id) return;
        await carregarDetalhes(id);
        await carregarGraph(`/Home/GetGraphItem?id=${id}`);
    });

    inicializarGrafo();
    await carregarSelect("/Home/GetProdutos", produtoSelect, "produto");
    await carregarGraph("/Home/GetGraphProdutosIniciais");
});
