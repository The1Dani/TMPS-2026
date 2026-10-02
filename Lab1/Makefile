APP  = airport_manager
OS   ?= linux
ARCH ?= x64
EXT  = $(if $(filter win,$(OS)),.exe,)

build:
	dotnet publish -c Release -r $(OS)-$(ARCH) --self-contained \
		-p:PublishSingleFile=true -p:DebugType=none -o publish
	ln -sf publish/$(APP)$(EXT) ./$(APP)$(EXT)

run: build
	./$(APP)$(EXT)

clean:
	rm -rf bin obj publish $(APP) $(APP).exe

.PHONY: build run clean
