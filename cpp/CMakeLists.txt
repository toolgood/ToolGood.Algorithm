cmake_minimum_required(VERSION 3.16)
project(ToolGood.Algorithm.Cpp LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

file(GLOB_RECURSE TG_SOURCES CONFIGURE_DEPENDS "${CMAKE_CURRENT_SOURCE_DIR}/toolgood/*.cpp")

add_library(toolgood_algorithm STATIC ${TG_SOURCES})
target_include_directories(toolgood_algorithm PUBLIC "${CMAKE_CURRENT_SOURCE_DIR}")

if (MSVC)
    target_compile_options(toolgood_algorithm PRIVATE /W4 /utf-8)
else()
    target_compile_options(toolgood_algorithm PRIVATE -Wall -Wextra -Wno-unused-parameter)
endif()

# 可选：编译示例
# add_executable(demo examples/demo.cpp)
# target_link_libraries(demo PRIVATE toolgood_algorithm)

# 单元测试（864 个用例，移植自 csharp/ToolGood.Algorithm.Test）
#   cmake -S cpp -B build -DTG_BUILD_TESTS=ON && cmake --build build && ctest --test-dir build
option(TG_BUILD_TESTS "Build unit tests (ported from the C# test project)" OFF)
if (TG_BUILD_TESTS)
    enable_testing()
    file(GLOB TG_TEST_SOURCES CONFIGURE_DEPENDS
         "${CMAKE_CURRENT_SOURCE_DIR}/tests/main.cpp"
         "${CMAKE_CURRENT_SOURCE_DIR}/tests/*.cpp")
    add_executable(tg_tests ${TG_TEST_SOURCES})
    target_include_directories(tg_tests PRIVATE
                               "${CMAKE_CURRENT_SOURCE_DIR}"
                               "${CMAKE_CURRENT_SOURCE_DIR}/tests")
    target_link_libraries(tg_tests PRIVATE toolgood_algorithm)
    add_test(NAME tg_tests COMMAND tg_tests)
endif()
