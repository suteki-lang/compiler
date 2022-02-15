# Compile the suteki files 
cd source
dotnet run ../tests/test.su ../tests/libc.su ../tests/common.su

# Compile the output files
cd ..
g++ -I tests/out/ tests/out/files/**/*.cpp -fno-exceptions && ./a.out