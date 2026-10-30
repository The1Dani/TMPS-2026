// Renders ```mermaid blocks with mermaid's standard (light) theme instead of
// Material's built-in integration, so UML arrowheads keep their proper shapes
// (hollow triangles / diamonds) and stay readable in both light and dark mode.
import mermaid from "https://cdn.jsdelivr.net/npm/mermaid@11.4.1/dist/mermaid.esm.min.mjs";

mermaid.initialize({
  startOnLoad: false,
  theme: "default",
  securityLevel: "strict",
  themeVariables: { fontSize: "15px", lineColor: "#222" },
});

// Render one at a time with unique ids: rendering in parallel lets class
// diagrams that share class names overwrite each other's nodes.
const blocks = document.querySelectorAll(".uml");
for (const [i, el] of blocks.entries()) {
  const { svg } = await mermaid.render(`uml-${i}`, el.textContent);
  el.innerHTML = svg;
}
