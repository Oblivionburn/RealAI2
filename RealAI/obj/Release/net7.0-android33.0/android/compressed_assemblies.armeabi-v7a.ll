; ModuleID = 'obj\Release\net7.0-android33.0\android\compressed_assemblies.armeabi-v7a.ll'
source_filename = "obj\Release\net7.0-android33.0\android\compressed_assemblies.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.CompressedAssemblyDescriptor = type {
	i32,; uint32_t uncompressed_file_size
	i8,; bool loaded
	i8*; uint8_t* data
}

%struct.CompressedAssemblies = type {
	i32,; uint32_t count
	%struct.CompressedAssemblyDescriptor*; CompressedAssemblyDescriptor* descriptors
}
@__CompressedAssemblyDescriptor_data_0 = internal global [146944 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_1 = internal global [169872 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_2 = internal global [5632 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_3 = internal global [11776 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_4 = internal global [16384 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_5 = internal global [44032 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_6 = internal global [16384 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_7 = internal global [15872 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_8 = internal global [14336 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_9 = internal global [7680 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_10 = internal global [56320 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_11 = internal global [123840 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_12 = internal global [1636800 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_13 = internal global [36352 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_14 = internal global [205760 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_15 = internal global [596416 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_16 = internal global [1497088 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_17 = internal global [526848 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_18 = internal global [6144 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_19 = internal global [50688 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_20 = internal global [5632 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_21 = internal global [35840 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_22 = internal global [15360 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_23 = internal global [12800 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_24 = internal global [27648 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_25 = internal global [4608 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_26 = internal global [10752 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_27 = internal global [31232 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_28 = internal global [342016 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_29 = internal global [33280 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_30 = internal global [4608 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_31 = internal global [119296 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_32 = internal global [34304 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_33 = internal global [6656 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_34 = internal global [4608 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_35 = internal global [15872 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_36 = internal global [629760 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_37 = internal global [4096 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_38 = internal global [4608 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_39 = internal global [13312 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_40 = internal global [4096 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_41 = internal global [4608 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_42 = internal global [4608 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_43 = internal global [46080 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_44 = internal global [14336 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_45 = internal global [475136 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_46 = internal global [14848 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_47 = internal global [17408 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_48 = internal global [67072 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_49 = internal global [444928 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_50 = internal global [21504 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_51 = internal global [7680 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_52 = internal global [38400 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_53 = internal global [179712 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_54 = internal global [17920 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_55 = internal global [15360 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_56 = internal global [15872 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_57 = internal global [11264 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_58 = internal global [32768 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_59 = internal global [73728 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_60 = internal global [16384 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_61 = internal global [50176 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_62 = internal global [26112 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_63 = internal global [378880 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_64 = internal global [10240 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_65 = internal global [33792 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_66 = internal global [51200 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_67 = internal global [27136 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_68 = internal global [13824 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_69 = internal global [505856 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_70 = internal global [30208 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_71 = internal global [14336 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_72 = internal global [21504 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_73 = internal global [12800 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_74 = internal global [351232 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_75 = internal global [1503744 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_76 = internal global [65536 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_77 = internal global [61952 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_78 = internal global [12288 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_79 = internal global [286720 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_80 = internal global [4096 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_81 = internal global [21504 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_82 = internal global [12288 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_83 = internal global [351232 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_84 = internal global [1488896 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_85 = internal global [66048 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_86 = internal global [61440 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_87 = internal global [12288 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_88 = internal global [286208 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_89 = internal global [4096 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_90 = internal global [6144 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_91 = internal global [21504 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_92 = internal global [12288 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_93 = internal global [351232 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_94 = internal global [1488896 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_95 = internal global [66048 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_96 = internal global [61440 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_97 = internal global [12288 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_98 = internal global [286208 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_99 = internal global [4096 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_100 = internal global [21504 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_101 = internal global [13312 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_102 = internal global [351232 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_103 = internal global [1518080 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_104 = internal global [65536 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_105 = internal global [61952 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_106 = internal global [12288 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_107 = internal global [286720 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_108 = internal global [4096 x i8] zeroinitializer, align 1


; Compressed assembly data storage
@compressed_assembly_descriptors = internal global [109 x %struct.CompressedAssemblyDescriptor] [
	; 0
	%struct.CompressedAssemblyDescriptor {
		i32 146944, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([146944 x i8], [146944 x i8]* @__CompressedAssemblyDescriptor_data_0, i32 0, i32 0); data
	}, 
	; 1
	%struct.CompressedAssemblyDescriptor {
		i32 169872, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([169872 x i8], [169872 x i8]* @__CompressedAssemblyDescriptor_data_1, i32 0, i32 0); data
	}, 
	; 2
	%struct.CompressedAssemblyDescriptor {
		i32 5632, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([5632 x i8], [5632 x i8]* @__CompressedAssemblyDescriptor_data_2, i32 0, i32 0); data
	}, 
	; 3
	%struct.CompressedAssemblyDescriptor {
		i32 11776, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([11776 x i8], [11776 x i8]* @__CompressedAssemblyDescriptor_data_3, i32 0, i32 0); data
	}, 
	; 4
	%struct.CompressedAssemblyDescriptor {
		i32 16384, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([16384 x i8], [16384 x i8]* @__CompressedAssemblyDescriptor_data_4, i32 0, i32 0); data
	}, 
	; 5
	%struct.CompressedAssemblyDescriptor {
		i32 44032, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([44032 x i8], [44032 x i8]* @__CompressedAssemblyDescriptor_data_5, i32 0, i32 0); data
	}, 
	; 6
	%struct.CompressedAssemblyDescriptor {
		i32 16384, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([16384 x i8], [16384 x i8]* @__CompressedAssemblyDescriptor_data_6, i32 0, i32 0); data
	}, 
	; 7
	%struct.CompressedAssemblyDescriptor {
		i32 15872, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15872 x i8], [15872 x i8]* @__CompressedAssemblyDescriptor_data_7, i32 0, i32 0); data
	}, 
	; 8
	%struct.CompressedAssemblyDescriptor {
		i32 14336, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([14336 x i8], [14336 x i8]* @__CompressedAssemblyDescriptor_data_8, i32 0, i32 0); data
	}, 
	; 9
	%struct.CompressedAssemblyDescriptor {
		i32 7680, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([7680 x i8], [7680 x i8]* @__CompressedAssemblyDescriptor_data_9, i32 0, i32 0); data
	}, 
	; 10
	%struct.CompressedAssemblyDescriptor {
		i32 56320, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([56320 x i8], [56320 x i8]* @__CompressedAssemblyDescriptor_data_10, i32 0, i32 0); data
	}, 
	; 11
	%struct.CompressedAssemblyDescriptor {
		i32 123840, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([123840 x i8], [123840 x i8]* @__CompressedAssemblyDescriptor_data_11, i32 0, i32 0); data
	}, 
	; 12
	%struct.CompressedAssemblyDescriptor {
		i32 1636800, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1636800 x i8], [1636800 x i8]* @__CompressedAssemblyDescriptor_data_12, i32 0, i32 0); data
	}, 
	; 13
	%struct.CompressedAssemblyDescriptor {
		i32 36352, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([36352 x i8], [36352 x i8]* @__CompressedAssemblyDescriptor_data_13, i32 0, i32 0); data
	}, 
	; 14
	%struct.CompressedAssemblyDescriptor {
		i32 205760, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([205760 x i8], [205760 x i8]* @__CompressedAssemblyDescriptor_data_14, i32 0, i32 0); data
	}, 
	; 15
	%struct.CompressedAssemblyDescriptor {
		i32 596416, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([596416 x i8], [596416 x i8]* @__CompressedAssemblyDescriptor_data_15, i32 0, i32 0); data
	}, 
	; 16
	%struct.CompressedAssemblyDescriptor {
		i32 1497088, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1497088 x i8], [1497088 x i8]* @__CompressedAssemblyDescriptor_data_16, i32 0, i32 0); data
	}, 
	; 17
	%struct.CompressedAssemblyDescriptor {
		i32 526848, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([526848 x i8], [526848 x i8]* @__CompressedAssemblyDescriptor_data_17, i32 0, i32 0); data
	}, 
	; 18
	%struct.CompressedAssemblyDescriptor {
		i32 6144, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([6144 x i8], [6144 x i8]* @__CompressedAssemblyDescriptor_data_18, i32 0, i32 0); data
	}, 
	; 19
	%struct.CompressedAssemblyDescriptor {
		i32 50688, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([50688 x i8], [50688 x i8]* @__CompressedAssemblyDescriptor_data_19, i32 0, i32 0); data
	}, 
	; 20
	%struct.CompressedAssemblyDescriptor {
		i32 5632, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([5632 x i8], [5632 x i8]* @__CompressedAssemblyDescriptor_data_20, i32 0, i32 0); data
	}, 
	; 21
	%struct.CompressedAssemblyDescriptor {
		i32 35840, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([35840 x i8], [35840 x i8]* @__CompressedAssemblyDescriptor_data_21, i32 0, i32 0); data
	}, 
	; 22
	%struct.CompressedAssemblyDescriptor {
		i32 15360, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15360 x i8], [15360 x i8]* @__CompressedAssemblyDescriptor_data_22, i32 0, i32 0); data
	}, 
	; 23
	%struct.CompressedAssemblyDescriptor {
		i32 12800, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12800 x i8], [12800 x i8]* @__CompressedAssemblyDescriptor_data_23, i32 0, i32 0); data
	}, 
	; 24
	%struct.CompressedAssemblyDescriptor {
		i32 27648, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([27648 x i8], [27648 x i8]* @__CompressedAssemblyDescriptor_data_24, i32 0, i32 0); data
	}, 
	; 25
	%struct.CompressedAssemblyDescriptor {
		i32 4608, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4608 x i8], [4608 x i8]* @__CompressedAssemblyDescriptor_data_25, i32 0, i32 0); data
	}, 
	; 26
	%struct.CompressedAssemblyDescriptor {
		i32 10752, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([10752 x i8], [10752 x i8]* @__CompressedAssemblyDescriptor_data_26, i32 0, i32 0); data
	}, 
	; 27
	%struct.CompressedAssemblyDescriptor {
		i32 31232, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([31232 x i8], [31232 x i8]* @__CompressedAssemblyDescriptor_data_27, i32 0, i32 0); data
	}, 
	; 28
	%struct.CompressedAssemblyDescriptor {
		i32 342016, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([342016 x i8], [342016 x i8]* @__CompressedAssemblyDescriptor_data_28, i32 0, i32 0); data
	}, 
	; 29
	%struct.CompressedAssemblyDescriptor {
		i32 33280, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([33280 x i8], [33280 x i8]* @__CompressedAssemblyDescriptor_data_29, i32 0, i32 0); data
	}, 
	; 30
	%struct.CompressedAssemblyDescriptor {
		i32 4608, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4608 x i8], [4608 x i8]* @__CompressedAssemblyDescriptor_data_30, i32 0, i32 0); data
	}, 
	; 31
	%struct.CompressedAssemblyDescriptor {
		i32 119296, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([119296 x i8], [119296 x i8]* @__CompressedAssemblyDescriptor_data_31, i32 0, i32 0); data
	}, 
	; 32
	%struct.CompressedAssemblyDescriptor {
		i32 34304, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([34304 x i8], [34304 x i8]* @__CompressedAssemblyDescriptor_data_32, i32 0, i32 0); data
	}, 
	; 33
	%struct.CompressedAssemblyDescriptor {
		i32 6656, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([6656 x i8], [6656 x i8]* @__CompressedAssemblyDescriptor_data_33, i32 0, i32 0); data
	}, 
	; 34
	%struct.CompressedAssemblyDescriptor {
		i32 4608, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4608 x i8], [4608 x i8]* @__CompressedAssemblyDescriptor_data_34, i32 0, i32 0); data
	}, 
	; 35
	%struct.CompressedAssemblyDescriptor {
		i32 15872, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15872 x i8], [15872 x i8]* @__CompressedAssemblyDescriptor_data_35, i32 0, i32 0); data
	}, 
	; 36
	%struct.CompressedAssemblyDescriptor {
		i32 629760, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([629760 x i8], [629760 x i8]* @__CompressedAssemblyDescriptor_data_36, i32 0, i32 0); data
	}, 
	; 37
	%struct.CompressedAssemblyDescriptor {
		i32 4096, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4096 x i8], [4096 x i8]* @__CompressedAssemblyDescriptor_data_37, i32 0, i32 0); data
	}, 
	; 38
	%struct.CompressedAssemblyDescriptor {
		i32 4608, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4608 x i8], [4608 x i8]* @__CompressedAssemblyDescriptor_data_38, i32 0, i32 0); data
	}, 
	; 39
	%struct.CompressedAssemblyDescriptor {
		i32 13312, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([13312 x i8], [13312 x i8]* @__CompressedAssemblyDescriptor_data_39, i32 0, i32 0); data
	}, 
	; 40
	%struct.CompressedAssemblyDescriptor {
		i32 4096, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4096 x i8], [4096 x i8]* @__CompressedAssemblyDescriptor_data_40, i32 0, i32 0); data
	}, 
	; 41
	%struct.CompressedAssemblyDescriptor {
		i32 4608, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4608 x i8], [4608 x i8]* @__CompressedAssemblyDescriptor_data_41, i32 0, i32 0); data
	}, 
	; 42
	%struct.CompressedAssemblyDescriptor {
		i32 4608, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4608 x i8], [4608 x i8]* @__CompressedAssemblyDescriptor_data_42, i32 0, i32 0); data
	}, 
	; 43
	%struct.CompressedAssemblyDescriptor {
		i32 46080, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([46080 x i8], [46080 x i8]* @__CompressedAssemblyDescriptor_data_43, i32 0, i32 0); data
	}, 
	; 44
	%struct.CompressedAssemblyDescriptor {
		i32 14336, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([14336 x i8], [14336 x i8]* @__CompressedAssemblyDescriptor_data_44, i32 0, i32 0); data
	}, 
	; 45
	%struct.CompressedAssemblyDescriptor {
		i32 475136, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([475136 x i8], [475136 x i8]* @__CompressedAssemblyDescriptor_data_45, i32 0, i32 0); data
	}, 
	; 46
	%struct.CompressedAssemblyDescriptor {
		i32 14848, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([14848 x i8], [14848 x i8]* @__CompressedAssemblyDescriptor_data_46, i32 0, i32 0); data
	}, 
	; 47
	%struct.CompressedAssemblyDescriptor {
		i32 17408, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([17408 x i8], [17408 x i8]* @__CompressedAssemblyDescriptor_data_47, i32 0, i32 0); data
	}, 
	; 48
	%struct.CompressedAssemblyDescriptor {
		i32 67072, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([67072 x i8], [67072 x i8]* @__CompressedAssemblyDescriptor_data_48, i32 0, i32 0); data
	}, 
	; 49
	%struct.CompressedAssemblyDescriptor {
		i32 444928, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([444928 x i8], [444928 x i8]* @__CompressedAssemblyDescriptor_data_49, i32 0, i32 0); data
	}, 
	; 50
	%struct.CompressedAssemblyDescriptor {
		i32 21504, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([21504 x i8], [21504 x i8]* @__CompressedAssemblyDescriptor_data_50, i32 0, i32 0); data
	}, 
	; 51
	%struct.CompressedAssemblyDescriptor {
		i32 7680, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([7680 x i8], [7680 x i8]* @__CompressedAssemblyDescriptor_data_51, i32 0, i32 0); data
	}, 
	; 52
	%struct.CompressedAssemblyDescriptor {
		i32 38400, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([38400 x i8], [38400 x i8]* @__CompressedAssemblyDescriptor_data_52, i32 0, i32 0); data
	}, 
	; 53
	%struct.CompressedAssemblyDescriptor {
		i32 179712, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([179712 x i8], [179712 x i8]* @__CompressedAssemblyDescriptor_data_53, i32 0, i32 0); data
	}, 
	; 54
	%struct.CompressedAssemblyDescriptor {
		i32 17920, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([17920 x i8], [17920 x i8]* @__CompressedAssemblyDescriptor_data_54, i32 0, i32 0); data
	}, 
	; 55
	%struct.CompressedAssemblyDescriptor {
		i32 15360, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15360 x i8], [15360 x i8]* @__CompressedAssemblyDescriptor_data_55, i32 0, i32 0); data
	}, 
	; 56
	%struct.CompressedAssemblyDescriptor {
		i32 15872, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15872 x i8], [15872 x i8]* @__CompressedAssemblyDescriptor_data_56, i32 0, i32 0); data
	}, 
	; 57
	%struct.CompressedAssemblyDescriptor {
		i32 11264, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([11264 x i8], [11264 x i8]* @__CompressedAssemblyDescriptor_data_57, i32 0, i32 0); data
	}, 
	; 58
	%struct.CompressedAssemblyDescriptor {
		i32 32768, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([32768 x i8], [32768 x i8]* @__CompressedAssemblyDescriptor_data_58, i32 0, i32 0); data
	}, 
	; 59
	%struct.CompressedAssemblyDescriptor {
		i32 73728, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([73728 x i8], [73728 x i8]* @__CompressedAssemblyDescriptor_data_59, i32 0, i32 0); data
	}, 
	; 60
	%struct.CompressedAssemblyDescriptor {
		i32 16384, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([16384 x i8], [16384 x i8]* @__CompressedAssemblyDescriptor_data_60, i32 0, i32 0); data
	}, 
	; 61
	%struct.CompressedAssemblyDescriptor {
		i32 50176, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([50176 x i8], [50176 x i8]* @__CompressedAssemblyDescriptor_data_61, i32 0, i32 0); data
	}, 
	; 62
	%struct.CompressedAssemblyDescriptor {
		i32 26112, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([26112 x i8], [26112 x i8]* @__CompressedAssemblyDescriptor_data_62, i32 0, i32 0); data
	}, 
	; 63
	%struct.CompressedAssemblyDescriptor {
		i32 378880, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([378880 x i8], [378880 x i8]* @__CompressedAssemblyDescriptor_data_63, i32 0, i32 0); data
	}, 
	; 64
	%struct.CompressedAssemblyDescriptor {
		i32 10240, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([10240 x i8], [10240 x i8]* @__CompressedAssemblyDescriptor_data_64, i32 0, i32 0); data
	}, 
	; 65
	%struct.CompressedAssemblyDescriptor {
		i32 33792, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([33792 x i8], [33792 x i8]* @__CompressedAssemblyDescriptor_data_65, i32 0, i32 0); data
	}, 
	; 66
	%struct.CompressedAssemblyDescriptor {
		i32 51200, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([51200 x i8], [51200 x i8]* @__CompressedAssemblyDescriptor_data_66, i32 0, i32 0); data
	}, 
	; 67
	%struct.CompressedAssemblyDescriptor {
		i32 27136, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([27136 x i8], [27136 x i8]* @__CompressedAssemblyDescriptor_data_67, i32 0, i32 0); data
	}, 
	; 68
	%struct.CompressedAssemblyDescriptor {
		i32 13824, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([13824 x i8], [13824 x i8]* @__CompressedAssemblyDescriptor_data_68, i32 0, i32 0); data
	}, 
	; 69
	%struct.CompressedAssemblyDescriptor {
		i32 505856, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([505856 x i8], [505856 x i8]* @__CompressedAssemblyDescriptor_data_69, i32 0, i32 0); data
	}, 
	; 70
	%struct.CompressedAssemblyDescriptor {
		i32 30208, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([30208 x i8], [30208 x i8]* @__CompressedAssemblyDescriptor_data_70, i32 0, i32 0); data
	}, 
	; 71
	%struct.CompressedAssemblyDescriptor {
		i32 14336, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([14336 x i8], [14336 x i8]* @__CompressedAssemblyDescriptor_data_71, i32 0, i32 0); data
	}, 
	; 72
	%struct.CompressedAssemblyDescriptor {
		i32 21504, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([21504 x i8], [21504 x i8]* @__CompressedAssemblyDescriptor_data_72, i32 0, i32 0); data
	}, 
	; 73
	%struct.CompressedAssemblyDescriptor {
		i32 12800, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12800 x i8], [12800 x i8]* @__CompressedAssemblyDescriptor_data_73, i32 0, i32 0); data
	}, 
	; 74
	%struct.CompressedAssemblyDescriptor {
		i32 351232, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([351232 x i8], [351232 x i8]* @__CompressedAssemblyDescriptor_data_74, i32 0, i32 0); data
	}, 
	; 75
	%struct.CompressedAssemblyDescriptor {
		i32 1503744, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1503744 x i8], [1503744 x i8]* @__CompressedAssemblyDescriptor_data_75, i32 0, i32 0); data
	}, 
	; 76
	%struct.CompressedAssemblyDescriptor {
		i32 65536, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([65536 x i8], [65536 x i8]* @__CompressedAssemblyDescriptor_data_76, i32 0, i32 0); data
	}, 
	; 77
	%struct.CompressedAssemblyDescriptor {
		i32 61952, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([61952 x i8], [61952 x i8]* @__CompressedAssemblyDescriptor_data_77, i32 0, i32 0); data
	}, 
	; 78
	%struct.CompressedAssemblyDescriptor {
		i32 12288, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12288 x i8], [12288 x i8]* @__CompressedAssemblyDescriptor_data_78, i32 0, i32 0); data
	}, 
	; 79
	%struct.CompressedAssemblyDescriptor {
		i32 286720, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([286720 x i8], [286720 x i8]* @__CompressedAssemblyDescriptor_data_79, i32 0, i32 0); data
	}, 
	; 80
	%struct.CompressedAssemblyDescriptor {
		i32 4096, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4096 x i8], [4096 x i8]* @__CompressedAssemblyDescriptor_data_80, i32 0, i32 0); data
	}, 
	; 81
	%struct.CompressedAssemblyDescriptor {
		i32 21504, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([21504 x i8], [21504 x i8]* @__CompressedAssemblyDescriptor_data_81, i32 0, i32 0); data
	}, 
	; 82
	%struct.CompressedAssemblyDescriptor {
		i32 12288, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12288 x i8], [12288 x i8]* @__CompressedAssemblyDescriptor_data_82, i32 0, i32 0); data
	}, 
	; 83
	%struct.CompressedAssemblyDescriptor {
		i32 351232, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([351232 x i8], [351232 x i8]* @__CompressedAssemblyDescriptor_data_83, i32 0, i32 0); data
	}, 
	; 84
	%struct.CompressedAssemblyDescriptor {
		i32 1488896, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1488896 x i8], [1488896 x i8]* @__CompressedAssemblyDescriptor_data_84, i32 0, i32 0); data
	}, 
	; 85
	%struct.CompressedAssemblyDescriptor {
		i32 66048, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([66048 x i8], [66048 x i8]* @__CompressedAssemblyDescriptor_data_85, i32 0, i32 0); data
	}, 
	; 86
	%struct.CompressedAssemblyDescriptor {
		i32 61440, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([61440 x i8], [61440 x i8]* @__CompressedAssemblyDescriptor_data_86, i32 0, i32 0); data
	}, 
	; 87
	%struct.CompressedAssemblyDescriptor {
		i32 12288, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12288 x i8], [12288 x i8]* @__CompressedAssemblyDescriptor_data_87, i32 0, i32 0); data
	}, 
	; 88
	%struct.CompressedAssemblyDescriptor {
		i32 286208, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([286208 x i8], [286208 x i8]* @__CompressedAssemblyDescriptor_data_88, i32 0, i32 0); data
	}, 
	; 89
	%struct.CompressedAssemblyDescriptor {
		i32 4096, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4096 x i8], [4096 x i8]* @__CompressedAssemblyDescriptor_data_89, i32 0, i32 0); data
	}, 
	; 90
	%struct.CompressedAssemblyDescriptor {
		i32 6144, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([6144 x i8], [6144 x i8]* @__CompressedAssemblyDescriptor_data_90, i32 0, i32 0); data
	}, 
	; 91
	%struct.CompressedAssemblyDescriptor {
		i32 21504, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([21504 x i8], [21504 x i8]* @__CompressedAssemblyDescriptor_data_91, i32 0, i32 0); data
	}, 
	; 92
	%struct.CompressedAssemblyDescriptor {
		i32 12288, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12288 x i8], [12288 x i8]* @__CompressedAssemblyDescriptor_data_92, i32 0, i32 0); data
	}, 
	; 93
	%struct.CompressedAssemblyDescriptor {
		i32 351232, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([351232 x i8], [351232 x i8]* @__CompressedAssemblyDescriptor_data_93, i32 0, i32 0); data
	}, 
	; 94
	%struct.CompressedAssemblyDescriptor {
		i32 1488896, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1488896 x i8], [1488896 x i8]* @__CompressedAssemblyDescriptor_data_94, i32 0, i32 0); data
	}, 
	; 95
	%struct.CompressedAssemblyDescriptor {
		i32 66048, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([66048 x i8], [66048 x i8]* @__CompressedAssemblyDescriptor_data_95, i32 0, i32 0); data
	}, 
	; 96
	%struct.CompressedAssemblyDescriptor {
		i32 61440, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([61440 x i8], [61440 x i8]* @__CompressedAssemblyDescriptor_data_96, i32 0, i32 0); data
	}, 
	; 97
	%struct.CompressedAssemblyDescriptor {
		i32 12288, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12288 x i8], [12288 x i8]* @__CompressedAssemblyDescriptor_data_97, i32 0, i32 0); data
	}, 
	; 98
	%struct.CompressedAssemblyDescriptor {
		i32 286208, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([286208 x i8], [286208 x i8]* @__CompressedAssemblyDescriptor_data_98, i32 0, i32 0); data
	}, 
	; 99
	%struct.CompressedAssemblyDescriptor {
		i32 4096, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4096 x i8], [4096 x i8]* @__CompressedAssemblyDescriptor_data_99, i32 0, i32 0); data
	}, 
	; 100
	%struct.CompressedAssemblyDescriptor {
		i32 21504, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([21504 x i8], [21504 x i8]* @__CompressedAssemblyDescriptor_data_100, i32 0, i32 0); data
	}, 
	; 101
	%struct.CompressedAssemblyDescriptor {
		i32 13312, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([13312 x i8], [13312 x i8]* @__CompressedAssemblyDescriptor_data_101, i32 0, i32 0); data
	}, 
	; 102
	%struct.CompressedAssemblyDescriptor {
		i32 351232, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([351232 x i8], [351232 x i8]* @__CompressedAssemblyDescriptor_data_102, i32 0, i32 0); data
	}, 
	; 103
	%struct.CompressedAssemblyDescriptor {
		i32 1518080, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1518080 x i8], [1518080 x i8]* @__CompressedAssemblyDescriptor_data_103, i32 0, i32 0); data
	}, 
	; 104
	%struct.CompressedAssemblyDescriptor {
		i32 65536, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([65536 x i8], [65536 x i8]* @__CompressedAssemblyDescriptor_data_104, i32 0, i32 0); data
	}, 
	; 105
	%struct.CompressedAssemblyDescriptor {
		i32 61952, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([61952 x i8], [61952 x i8]* @__CompressedAssemblyDescriptor_data_105, i32 0, i32 0); data
	}, 
	; 106
	%struct.CompressedAssemblyDescriptor {
		i32 12288, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12288 x i8], [12288 x i8]* @__CompressedAssemblyDescriptor_data_106, i32 0, i32 0); data
	}, 
	; 107
	%struct.CompressedAssemblyDescriptor {
		i32 286720, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([286720 x i8], [286720 x i8]* @__CompressedAssemblyDescriptor_data_107, i32 0, i32 0); data
	}, 
	; 108
	%struct.CompressedAssemblyDescriptor {
		i32 4096, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4096 x i8], [4096 x i8]* @__CompressedAssemblyDescriptor_data_108, i32 0, i32 0); data
	}
], align 4; end of 'compressed_assembly_descriptors' array


; compressed_assemblies
@compressed_assemblies = local_unnamed_addr global %struct.CompressedAssemblies {
	i32 109, ; count
	%struct.CompressedAssemblyDescriptor* getelementptr inbounds ([109 x %struct.CompressedAssemblyDescriptor], [109 x %struct.CompressedAssemblyDescriptor]* @compressed_assembly_descriptors, i32 0, i32 0); descriptors
}, align 4


!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/release/7.0.1xx @ 8f1d9a47205ead80132661f68b0cee9ed0e0220b"}
