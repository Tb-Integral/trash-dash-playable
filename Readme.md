# Trash Dash → Playable Ad

Portfolio case study: turning Unity Technologies' **Endless Runner Sample Game (Trash Dash)** into a production-style **playable ad**, while keeping the original game as a measured baseline.

This is a learning / portfolio derivative. It is **not** an official Unity project and **not** a redistributable copy of Trash Dash as a standalone product.

| | |
|---|---|
| Unity | **2021.3.6f1** (original GitHub readme still says 2019.3 — that is outdated) |
| Render pipeline | URP 12.1.7 |
| Content | Addressables 1.19.19 |
| Playable concept | **B** — authored ~20s sequence (not a timer on endless mode) |
| Status | Original baseline frozen. Playable gameplay not started yet. |

## Attribution

- Original game: [Unity-Technologies/EndlessRunnerSampleGame](https://github.com/Unity-Technologies/EndlessRunnerSampleGame)
- Asset Store listing: [Endless Runner Sample Game](https://assetstore.unity.com/packages/templates/tutorials/endless-runner-sample-game-87901)
- Author of the source project: **Unity Technologies**

## License (read this before forking)

The original GitHub repository **does not include a `LICENSE` file**. Default copyright therefore applies to that copy.

The Asset Store listing is under the **Standard Unity Asset Store EULA**. That EULA typically allows using assets **inside your own projects**, and restricts redistributing the assets as a standalone package. This README is not legal advice.

Third-party notice in the project (`Assets/EndlessRunner_Third-PartyNotice.txt`):

- **Luckiest Guy** font — Apache License 2.0

If you reuse this repo, keep Unity attribution and do not claim the original Trash Dash art, audio, or design as your own IP.

## What this case study will contain

1. Original mobile game, unmodified gameplay
2. WebGL baseline (size, load, FPS, memory)
3. Playable concept (scripted sequence)
4. Tutorial / win / fail / end card / CTA
5. Optimization pass with before/after numbers
6. Notes on WebGL vs Unity Playworks vs MRAID vs ad networks

## Playable concept (chosen)

**Option B — authored sequence**, using Trash Dash mechanics:

Intro → one swipe hint → collect coins (lane change) → jump an obstacle → slide under an obstacle → Win / Fail → End Card → PLAY NOW

Not used: a countdown on the real endless generator. Playable ads need a reproducible beat, not a random 20-second clip.

## WebGL baseline (original game, no optimization)

Platform: WebGL, compression **Gzip**, Quality **Fantastic**.  
`Application.targetFrameRate = 30` is set in code; it did **not** cap the browser (measured ~50 FPS).

WebGL input in this project is **touch/swipe only**. Keyboard arrows work in the Editor (`UNITY_EDITOR` / `UNITY_STANDALONE`) and are compiled out of the WebGL player. Desktop Chrome needs Device Mode (touch emulation) or a phone.

| Metric | Development | Release | Notes |
|---|---|---|---|
| Full build | 92.92 MB | **51.74 MB** | On disk / localhost |
| `.wasm` / `.wasm.gz` | 33.9 MB | **7.1 MB** | IL2CPP code |
| `.data` / `.data.gz` | 49.5 MB | **35.7 MB** | Player assets |
| `StreamingAssets` | 8.97 MB | **8.97 MB** | Addressables (same in both) |
| Time to splash | — | **1.39 s** | Localhost (not 4G download) |
| Time to game menu | — | **3.46 s** | Localhost |
| FPS during run | — | **~50** | Chrome, after tutorial |
| JS / “used” memory | — | **~2 MB** | Narrow heap counter |
| Chrome tab memory | — | **~122 MB** | Closer to device cost |

Localhost load time is **engine startup + WASM compile**, not downloading 52 MB. On mobile data the payload size dominates.

A typical playable network cap is about **5 MB** (Unity Ads / AppLovin / ironSource HTML; Meta HTML often **2 MB**, ZIP **5 MB**). This project's Release **`.wasm` alone is 7.1 MB**, so a raw Unity WebGL player is not an uploadable playable. Production packaging goes through **Unity Playworks** (or equivalent), not `File → Build Settings → WebGL` alone. This repo's WebGL builds are the **gameplay prototype and measurement track**.

## How to open the original game

1. Unity Hub → Unity **2021.3.6f1** with **WebGL Build Support**
2. Open this folder
3. Play `Assets/Scenes/Start.unity` (or press Play if that scene is loaded)
4. Before a WebGL player build: `Window → Asset Management → Addressables → Groups` → **Build → New Build → Default Build Script** (must match the active platform)

This repository uses **Git LFS** for large binaries (textures, audio, FBX), same as the upstream project.

```
git lfs install
```

## Original project notes (upstream)

The source is a mobile endless runner. The GitHub sample (vs older Asset Store package) includes:

- First-run tutorial
- Lightweight / Universal Render Pipeline
- Addressables instead of legacy Asset Bundles

Upstream still documents Addressable builds and services in `Assets/INSTRUCTIONS.txt`.

Wiki: [EndlessRunnerSampleGame wiki](https://github.com/Unity-Technologies/EndlessRunnerSampleGame/wiki)

## Optimization log

None yet. Baseline above is the **before** column. After playable gameplay exists, each change will be measured against this table.
