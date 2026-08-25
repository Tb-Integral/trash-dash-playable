# Trash Dash → Playable Ad

Portfolio case: Unity Technologies' [Endless Runner Sample Game (Trash Dash)](https://github.com/Unity-Technologies/EndlessRunnerSampleGame) turned into a short **playable ad**, while the original game still runs in the Editor.

Not an official Unity product. Not a redistributable copy of Trash Dash.

**Stack:** Unity 2021.3.6f1, URP 12.1.7, Addressables 1.19.19, Unity Playworks (Luna) 7.2.

## Demo

[Gameplay recording](https://drive.google.com/drive/folders/1rk66ak8Ov6HvpYo0GpTJYGafRMnfKN7T?usp=drive_link)

## What you play

Portrait, authored ~20–25 s (not endless):

Intro → change lane → jump → slide → win or fail → end card → **PLAY NOW**

Trash Cat, Day theme, one life. Shop, extra characters, Night, power-ups, and the full HUD stay in the project for the original game; they are off this run.

CTA in Playworks uses `Luna.Unity.Playable.InstallFullGame()`. In the Editor it opens the [upstream sample](https://github.com/Unity-Technologies/EndlessRunnerSampleGame).

## Size

Unity WebGL, Gzip, Quality Fantastic. Folder on disk (not a Playworks HTML).

| Build | Size | Notes |
|---|---|---|
| Original game (`Start`), Release | **51.7 MB** | `.wasm` 7.1 MB, `.data` 35.7 MB, StreamingAssets 9.0 MB |
| Playable scenes only, after size pass | **18.9 MB** | Same Unity player, fewer clips/textures |

Still over typical ad-network caps (**2–5 MB**). The playable is therefore exported with **Unity Playworks**, not `File → Build Settings → WebGL`.

Playworks also does not behave like the Unity Editor: jump/slide are sampled from clips because Mecanim does not switch those states; Luckiest Guy is FontBM'd from the TTF (Unity's dynamic atlas is invisible); audio starts after the first tap (Chrome autoplay).

## Open in Unity

Unity Hub → **2021.3.6f1** → this folder. Large binaries use **Git LFS** (`git lfs install`).

| | Scene |
|---|---|
| Playable | `Assets/Scenes/Playable.unity` |
| Original game | `Assets/Scenes/Start.unity` |

Build Settings scene 0 is Playable. For the full game, open `Start` and press Play.

Ad HTML: Unity Playworks Plugin → develop/release export (not the Unity WebGL player).

## Attribution and license

- Original game: [Unity-Technologies/EndlessRunnerSampleGame](https://github.com/Unity-Technologies/EndlessRunnerSampleGame)
- Asset Store: [Endless Runner Sample Game](https://assetstore.unity.com/packages/templates/tutorials/endless-runner-sample-game-87901)
- Author of the source: **Unity Technologies**

The upstream GitHub repo has **no `LICENSE` file** (copyright applies). The Asset Store listing uses the **Standard Unity Asset Store EULA** (use inside your own projects; do not ship the assets as a standalone package). This README is not legal advice.

**Luckiest Guy** font: Apache License 2.0 (`Assets/EndlessRunner_Third-PartyNotice.txt`).

Keep Unity attribution. Do not claim the original art, audio, or design as your own.
