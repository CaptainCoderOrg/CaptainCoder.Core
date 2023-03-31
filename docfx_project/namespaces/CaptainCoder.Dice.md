---
uid: CaptainCoder.Dice
summary: *content
---

## Overview

This module provides a set of classes for generating randomness using dice
notation. For example, the following code rolls a three six sided dice and
reports the result:

```csharp
DiceNotation threeDeeSix = DiceNotation.Parse("3d6");
RollResult result = threeDeeSix.Roll(null);
Console.WriteLine($"Rolled a: {result.Value}");
Console.WriteLine($"Detailed report: {result.Message}");
```

## Supported Notation

The dice notation parser supports integers, dice groups, variable identifiers,
addition, subtraction, multiplication, division, and parenthesis. For example:

```csharp
DiceNotation strengthCheck = DiceNotation.Parse("1d20 * (5 + Strength) - Health / 1d4");
```

To perform a roll with variables, you must provide an instance of `IRollContext`
which provides a single `int Lookup(string id);` method which is used to
evaluate each variable in the expression. If the variable cannot be resolved
during a roll, an exception may be thrown. For convenience, an extension method
`ToRollContext()` has been provided for `Dictionary<string, int>`.

```csharp
Dictionary<string, int> context = ...;
DiceNotation strengthCheck = DiceNotation.Parse("1d20 - Strength");
RollResult result = strengthCheck.Roll(context.ToRollContext());
Console.WriteLine($"Rolled a: {result.Value}");
Console.WriteLine($"Detailed report: {result.Message}");
```

## Conveniences

### String Extensions

For convenience, strings are provided 3 extension methods: `ToDiceNotation`, `SimpleRoll` and `Roll(context)`.

```csharp
IRandom randomSource = ...;
IRollContext context = ...;

DiceNotation strength = "3d6 + Strength".ToDiceNotation();
DiceNotation strengthWithRandomness = "3d6 + Strength".ToDiceNotation(randomSource);

// SimpleRoll() can be used for any roll that does not use variables
RollResult simpleRoll = "3d6 + 2".SimpleRoll();
RollResult simpleRollWithRandomness = "2d4".SimpleRoll(randomSource);

// Roll(context) requires a context
RollResult strengthRoll = "3d6 + Strength".Roll(context);
RollResult strengthRollWithRandomnexx = "3d6 + Strength".Roll(context, randomSource);
```

### Using RollResult as an int

The `DiceNotation.Roll` method, returns a `RollResult`. For convenience, the
`RollResult` class is implicitly be cast to an `int` within arithmetic
expressions. The cast is shorthand for using the `.Value` property:

```csharp
IRollContext context = ...;
DiceNotation weaponRoll = ...;
int damage = 2 * weaponRoll.Roll(context);
```

### Specifying the Source of Randomness

By default, the `Roll` method uses an instance of `IRandom` which wraps a shared
instance of the `System.Random` class. Both the `DiceNotation.Parse` and the
`DiceNotation.Roll` methods provide an overloaded method call for specifying the
source of randomness.

```csharp
IRandom randomSource = ...;
DiceNotation dice = DiceNotation.Parse("2d4 + 1d6 + 2", randomSource);
// Uses randomSource
RollResult result = dice.Roll(null);

IRandom riggedSource = ...;
// Uses the riggredSource
RollResult riggedResult = dice.Roll(null, riggedSource);
```