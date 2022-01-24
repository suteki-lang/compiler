## Suteki
**suteki (素敵)** is a simple statically typed systems programming language like [**C**](https://en.wikipedia.org/wiki/C_(programming_language)), it has features like:

* Modules
* Macros

and many more.

## Hello, World! example
(using [**D**](https://en.wikipedia.org/wiki/D_(programming_language)) syntax highlighting)
```d
// Export this file as 'hello_world' module
export hello_world;

// Import all public stuff from IO module
import standard.io;

// This is the entry point function where everything begins
void main()
{
    // This writes 'Hello, World!\n' to the console output
    console_writeln("Hello, World!");
}
```
See more examples [**here**](https://github.com/suteki-lang/examples).

## Building from source
TODO

## License
This compiler is distributed under the [**MIT License**](https://opensource.org/licenses/MIT). See [**LICENSE**](https://github.com/suteki-lang/compiler/blob/main/LICENSE) for more details.