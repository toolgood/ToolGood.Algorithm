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
