# Trash Dash → Playable Ad

Portfolio case study: turning Unity Technologies' **Endless Runner Sample Game (Trash Dash)** into a production-style **playable ad**, while keeping the original game as a measured baseline.

This is a learning / portfolio derivative. It is **not** an official Unity project and **not** a redistributable copy of Trash Dash as a standalone product.

| | |
|---|---|
| Unity | **2021.3.6f1** (original GitHub readme still says 2019.3 — that is outdated) |
| Render pipeline | URP 12.1.7 |
| Content | Addressables 1.19.19 |
| Playable concept | **B** — authored ~20s sequence (not a timer on endless mode) |
| Status | Gameplay prototype playable in Editor. Size / Playworks packaging not started. |

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

## Case study

| | |
|---|---|
| Original game + WebGL baseline | Done (table below) |
| Authored playable sequence | Done (Editor prototype) |
| Swipe hint / win / fail / end card / CTA | Done |
| Strip shop, extra characters, unused themes | Not started |
| Size optimization vs baseline | Not started |
| Unity Playworks / network MRAID wrapper | Not started |

## Playable prototype

**Option B — authored sequence**, using Trash Dash mechanics:

Intro → one swipe hint → collect coins (lane change) → jump → slide → Win / Fail → End Card → PLAY NOW

The original endless `TrackManager` is not copied. A playable-only queue feeds it Addressable tutorial segments (Cat, Day industrial). The last queued pieces are a horizon tail (no new obstacles) so the world does not clip. Win fires before the bait segment: **CONTINUE?** sits over a street that still shows coins and an obstacle. Fail uses the same end card (**TRY AGAIN!**) and the same CTA — a playable always ends on the store click.

CTA (`Assets/Plugins/WebGL/PlayableCTA.jslib`): `mraid.open` when the MRAID host exists, otherwise `window.open`. In the Editor it uses `Application.OpenURL`. Current URL points at the [upstream sample](https://github.com/Unity-Technologies/EndlessRunnerSampleGame).

Input:

- **Playable:** swipe (touch) and mouse-drag with the same 1% screen-width threshold. Keyboard still works in the Editor.
- **Original game (`Start.unity`):** unchanged. WebGL build of the original path is still touch-only; desktop Chrome needs Device Mode or a phone.

One life. Full-game HUD (pause, three hearts, coins, score) is hidden on the playable path. Shop / IAP / ads-revive / Raccoon / Night / power-ups are still in the project; they are not on this run.

Code lives in `Assets/Scripts/Playable/`. `Start.unity` → Main / Shop is the original game.

This is a **gameplay prototype**, not a 5 MB network creative. Do not treat `File → Build Settings → WebGL` output as an uploadable playable.

## How to open

Unity Hub → Unity **2021.3.6f1** with **WebGL Build Support** → open this folder.

| | Scene | What you get |
|---|---|---|
| Playable | `Assets/Scenes/Playable.unity` | Authored run, hint, end card |
| Original game | `Assets/Scenes/Start.unity` | Full Trash Dash (loadout, shop, endless) |

Build Settings scene 0 is **Playable**, so a player build launches the ad path. To play the original game, open `Start.unity` and press Play (do not rely on the default build index).

Before a WebGL player build: `Window → Asset Management → Addressables → Groups` → **Build → New Build → Default Build Script** (must match the active platform).

This repository uses **Git LFS** for large binaries (textures, audio, FBX), same as the upstream project.

```
git lfs install
```

## WebGL baseline (original game, no optimization)

Platform: WebGL, compression **Gzip**, Quality **Fantastic**.  
`Application.targetFrameRate = 30` is set in code; it did **not** cap the browser (measured ~50 FPS).

Measured on the **original** `Start.unity` loop, not the playable scene.

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

A typical playable network cap is about **5 MB** (Unity Ads / AppLovin / ironSource HTML; Meta HTML often **2 MB**, ZIP **5 MB**). This project's Release **`.wasm` alone is 7.1 MB**, so a raw Unity WebGL player is not an uploadable playable. Production packaging goes through **Unity Playworks** (or equivalent), not WebGL export alone. Builds from this repo are the **gameplay prototype and measurement track**.

Quality **Fantastic** and the WebGL heap limit (256 MB) are still the baseline settings; they have not been tuned for the playable.

## Original project notes (upstream)

The source is a mobile endless runner. The GitHub sample (vs older Asset Store package) includes:

- First-run tutorial
- Lightweight / Universal Render Pipeline
- Addressables instead of legacy Asset Bundles

Upstream still documents Addressable builds and services in `Assets/INSTRUCTIONS.txt`.

Wiki: [EndlessRunnerSampleGame wiki](https://github.com/Unity-Technologies/EndlessRunnerSampleGame/wiki)

## Optimization log

None yet. The table above is the **before** column, taken on the original game. Playable gameplay exists; the next measurement pass will be against that same table after content strip and WebGL settings change.
