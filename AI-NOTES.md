# 🤖 AI-NOTES.md — How AI Was Used in This Project

This file is a transparent log of where and how AI tooling (Claude, Copilot, etc.) assisted in building `league-api`. It exists because **AI-assisted development and AI-driven development are not the same thing**, and I want to be clear about which one this is.

---

## My Approach

I use AI as a collaborator, not a ghostwriter. I define the problem, evaluate the output critically, and own every decision. AI accelerates the journey — it doesn't choose the destination.

When AI suggested something I didn't understand, I asked questions until I did. When it suggested something wrong, I caught it and corrected it. This log documents that process.

---

## Log

### Environment Setup — Package Versions
**What happened:** The initial scaffold suggested PostgreSQL packages. I have SQL Server. The suggested NuGet packages defaulted to versions requiring .NET 10. I'm on .NET 8.

**What I changed:** Swapped Npgsql for Microsoft.EntityFrameworkCore.SqlServer. Pinned all EF Core packages to 9.0.x to match my .NET 8 environment.

**Why it matters:** Two cases where following AI output blindly would have broken the build before writing a single line of logic. Reading the output critically rather than accepting it is non-negotiable.

---

### Player Model — Race Length Cap
**What happened:** AI suggested capping race length at 100.

**What I changed:** Changed to 20.

**Why it matters:** No league race exceeds 20 games. The AI doesn't know billiards — I do. Domain knowledge overrides generated defaults.

---

### Email Duplicate Check — Case Normalization
**What happened:** AI generated a case-insensitive duplicate email check using `.ToLower()` at query time.

**What I changed:** Emails are now normalized to lowercase on input (`dto.Email.ToLower()`) and the duplicate check uses exact match. The error message returns the original input so the caller knows what they typed.

**Why it matters:** Emails are technically case-sensitive at the protocol level. Case-insensitive comparisons are technically wrong. Normalizing on input is cleaner than compensating at query time — consistent data storage is preferable to runtime workarounds.

---

### Match Validation — Handicap Scoring Rules
**What happened:** AI would have generated basic existence checks for POST /api/matches.

**What I added:** Winner's score must equal their race. Loser's score must be less than their race. These rules came entirely from billiards domain knowledge.

**Why it matters:** This is the difference between an API that accepts data and an API that understands its domain. No amount of prompting produces correct billiards league validation without the developer knowing the sport.

---

### Forfeit Handling — Auto-Setting Winner Score
**What happened:** Initial implementation required the caller to explicitly set the winning player's score equal to their race on a forfeit.

**What I changed:** Winner's score is now auto-set to their race when a forfeit is recorded. The caller only needs to provide who forfeited and who won.

**Why it matters:** The API should enforce and auto-set predictable values rather than requiring the consumer to know internal business rules. Reduces caller complexity and eliminates a category of invalid input entirely.

---

### Standings Tiebreaker — Last Name Sort
**What happened:** AI generated tiebreaker sort by full player name (first + last).

**What I changed:** Tiebreaker sorts by last name. Added LastName as a separate field on StandingDTO.

**Why it matters:** Last name alphabetical is standard for any sports standings board. Domain convention drove the correction.

---

### Hard Delete vs Soft Delete — Context Matters
**What happened:** AI applied the same deletion pattern to both Players and Matches.

**What I decided:** Players use soft delete (IsActive flag) because league players go inactive and return. Matches use hard delete because an incorrectly recorded match should simply not exist.

**Why it matters:** The right deletion strategy depends on what the data represents, not a one-size-fits-all pattern. Same application, two different strategies, both driven by domain logic.