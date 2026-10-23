// ConditionTree.cpp - 对应 csharp/ToolGood.Algorithm/Internals/ConditionTree.cs
#include "ConditionTree.h"

#include "../Lexer.h"

namespace toolgood {

void ConditionTree::SetSource(std::shared_ptr<const std::vector<uint32_t>> source) {
    source_ = std::move(source);
    text_.clear();
    textReady_ = false;
}

std::string ConditionTree::Text() const {
    if (!textReady_) {
        textReady_ = true;
        if (source_ && End >= Start && Start >= 0 && End < static_cast<int>(source_->size())) {
            text_ = Lexer::FromCodePoints(*source_, static_cast<size_t>(Start),
                                         static_cast<size_t>(End) + 1);
        }
    }
    return text_;
}

}  // namespace toolgood
