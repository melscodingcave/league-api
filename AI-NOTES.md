# 🤖 AI-NOTES.md — How AI Was Used in This Project

This file is a transparent log of where and how AI tooling (Claude, Copilot, etc.) assisted in building `league-api`. It exists because **AI-assisted development and AI-driven development are not the same thing**, and I want to be clear about which one this is.

---

## My Approach

I use AI as a collaborator, not a ghostwriter. I define the problem, evaluate the output critically, and own every decision. AI accelerates the journey — it doesn't choose the destination.

When AI suggested something I didn't understand, I asked questions until I did. When it suggested something wrong, I caught it and corrected it. This log documents that process.

---

## Log

### Data Model Design
*To be filled in as work progresses.*

Example format:
> **Prompt:** "Given a billiards league with players and match results, suggest an Entity Framework data model in C#."
>
> **What I used:** The `Player` and `Match` class structures, with modifications.
>
> **What I changed:** The AI generated a `Score` property as a single integer. I split this into `PlayerOneScore` and `PlayerTwoScore` as separate fields because a single score doesn't capture the match result clearly for standings computation.
>
> **Why it matters:** This is a domain modeling decision, not a syntax question. The AI doesn't know billiards — I do.

---

*This log will be updated as the project develops.*