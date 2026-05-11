# Scientific Calculator with Graphing (C# Windows Forms)

## Overview
This project is a desktop application developed using C# Windows Forms as part of an Object-Oriented Programming (OOP) course.  
It demonstrates both expression evaluation techniques and basic function graphing.

The project was developed under the supervision of Dr. Sherif Fadel Fahmy (Head of the Computer Department), AAST Sheraton.

---

## 📸 Demo / Screenshots
This shows the main interface of the calculator with graph plotting functionality.
![Calculator UI](images/calculator-ui.png)


## Features

### 🧮 Calculator Engine
- Infix to Postfix expression conversion
- Postfix expression evaluation
- Support for basic arithmetic operations:
  - Addition (+)
  - Subtraction (-)
  - Multiplication (*)
  - Division (/)
  - Power (^)
- Support for parentheses handling
- Unary minus (negative numbers support)

### 📊 Graphing Module
- Plot mathematical functions on a 2D coordinate system
- Supports:
  - sin(x)
  - cos(x)
  - tan(x)
  - General expressions involving x (e.g., x+5, 2*x+3)
- Scaled axis rendering
- Smooth curve drawing using point interpolation

---

## Technologies Used
- C#
- .NET Framework
- Windows Forms
- GDI+ (for drawing graphs)

---

## How It Works

1. User enters a mathematical expression.
2. The expression is converted from infix to postfix notation.
3. The postfix expression is evaluated using a stack-based algorithm.
4. For graphing:
   - The function is evaluated point-by-point
   - The result is plotted on a bitmap canvas inside a PictureBox

---

## Example Inputs

### Calculator Mode:
(3 + 5) * 2
2 ^ 3 + 4

### Graphing Mode:
sin(x)
cos(x)
x+5
2*x + 3


---

## How to Run
1. Open the `.sln` file in Visual Studio
2. Restore NuGet packages if needed
3. Build the solution
4. Run the application

---

## Notes
This project was developed as an academic assignment to demonstrate understanding of:
- Data structures (Stack)
- Expression parsing
- Object-Oriented Programming principles
- Event-driven programming
- Basic computer graphics rendering

---

## Author
Karim Mohamed Zaki
